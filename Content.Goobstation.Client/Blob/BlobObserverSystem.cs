// SPDX-FileCopyrightText: 2024 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 2024 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 2024 Piras314 <p1r4s@proton.me>
// SPDX-FileCopyrightText: 2024 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 2025 Aiden <28298836+Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 2025 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Goobstation.Common.Blob;
using Content.Goobstation.Shared.Blob;
using Content.Goobstation.Shared.Blob.Components;
using Content.Shared.GameTicking;
using Content.Shared.StatusIcon;
using Content.Shared.StatusIcon.Components;
using Robust.Client.Graphics;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
//using Content.Shared.Flesh;

namespace Content.Goobstation.Client.Blob;

public sealed class BlobObserverSystem : SharedBlobObserverSystem
{
    [Dependency] private readonly ILightManager _lightManager = default!;
    [Dependency] private readonly IPrototypeManager _prototype = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<BlobObserverComponent, LocalPlayerAttachedEvent>(OnPlayerAttached);
        SubscribeLocalEvent<BlobObserverComponent, LocalPlayerDetachedEvent>(OnPlayerDetached);

        SubscribeLocalEvent<BlobObserverComponent, GetStatusIconsEvent>(OnShowBlobIcon);
        SubscribeLocalEvent<ZombieBlobComponent, GetStatusIconsEvent>(OnShowBlobIcon);
        SubscribeLocalEvent<BlobCarrierComponent, GetStatusIconsEvent>(OnShowBlobIcon);
        SubscribeLocalEvent<BlobbernautComponent, GetStatusIconsEvent>(OnShowBlobIcon);

        SubscribeNetworkEvent<RoundRestartCleanupEvent>(RoundRestartCleanup);
    }

    [ValidatePrototypeId<FactionIconPrototype>]
    private const string BlobFaction = "BlobFaction";

    private void OnShowBlobIcon<T>(Entity<T> ent, ref GetStatusIconsEvent args) where T : Component
    {
        args.StatusIcons.Add(_prototype.Index<FactionIconPrototype>(BlobFaction));
    }

    private void OnPlayerAttached(EntityUid uid, BlobObserverComponent component, LocalPlayerAttachedEvent args)
    {
        _lightManager.DrawLighting = false;
    }

    private void OnPlayerDetached(EntityUid uid, BlobObserverComponent component, LocalPlayerDetachedEvent args)
    {
        _lightManager.DrawLighting = true;
    }

    private void RoundRestartCleanup(RoundRestartCleanupEvent ev)
    {
        _lightManager.DrawLighting = true;
    }
}