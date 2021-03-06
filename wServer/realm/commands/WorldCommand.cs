﻿#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using db;
using db.data;
using wServer.cliPackets;
using wServer.realm.entities;
using wServer.realm.entities.player;
using wServer.realm.worlds;
using wServer.svrPackets;

#endregion

namespace wServer.realm.commands
{
    internal class TutorialCommand : ICommand
    {
        public string Command
        {
            get { return "tutorial"; }
        }

        public int RequiredRank
        {
            get { return 0; }
        }

        public void Execute(Player player, string[] args)
        {
            player.Client.Reconnect(new ReconnectPacket
            {
                Host = "",
                Port = 2050,
                GameId = World.TUT_ID,
                Name = "Tutorial",
                Key = Empty<byte>.Array,
            });
        }
    }

    internal class TheWineler : ICommand
    {
        public string Command
        {
            get { return "shop"; }
        }

        public int RequiredRank
        {
            get { return 0; }
        }

        public void Execute(Player player, string[] args)
        {
            player.Client.Reconnect(new ReconnectPacket
            {
                Host = "",
                Port = 2050,
                GameId = World.WINELER,
                Name = "The Wineler",
                Key = Empty<byte>.Array,
            });
        }
    }
	
    internal class NexusVIP : ICommand
    {
        public string Command
        {
            get { return "nexusvip"; }
        }

        public int RequiredRank
        {
            get { return 3; }
        }

        public void Execute(Player player, string[] args)
        {
            if (player.Client.Account.Rank >= 3)
            {
                player.Client.Reconnect(new ReconnectPacket
                {
                    Host = "",
                    Port = 2050,
                    GameId = World.NEXUS_VIP,
                    Name = "Nexus VIP",
                    Key = Empty<byte>.Array,
                });
            } else 
                player.SendInfo("You need to be a VIP to use this command.");
        }
    }
	
    internal class NexusSv1 : ICommand
    {
        public string Command
        {
            get { return "nexust"; }
        }

        public int RequiredRank
        {
            get { return 11; }
        }

        public void Execute(Player player, string[] args)
        {
            player.Client.Reconnect(new ReconnectPacket
            {
                Host = "",
                Port = 2045,
                GameId = World.NEXUS_ID,
                Name = "Nexus (Testing sv2)",
                Key = Empty<byte>.Array,
            });
        }
    }
	
    internal class Godlands : ICommand
    {
        public string Command
        {
            get { return "glands"; }
        }

        public int RequiredRank
        {
            get { return 0; }
        }

        public void Execute(Player player, string[] args)
        {
            player.Move(1000, 1000);
            if (player.Pet != null)
                player.Pet.Move(1000, 1000);
            player.UpdateCount++;
            player.ApplyConditionEffect(new ConditionEffect
            {
                Effect = ConditionEffectIndex.Invulnerable,
                DurationMS = 5000
            });
            player.SendInfo("You are teleported to God Lands (Mountains).\nNow, you are protect during 5 seconds. It's not safe place to beginners less level 20...");
            player.Owner.BroadcastPacket(new GotoPacket
            {
                ObjectId = player.Id,
                Position = new Position
                {
                    X = player.X,
                    Y = player.Y
                }
            }
                , null);
        }
    }
	
    internal class PetTP : ICommand
    {
        public string Command
        {
            get { return "pet"; }
        }

        public int RequiredRank
        {
            get { return 0; }
        }

        public void Execute(Player player, string[] args)
        {
            if (player.Pet != null)
            {
                player.Pet.Move(player.X, player.Y);
                player.SendInfo("Your pet has been teleported near you!");
                player.UpdateCount++;
            } else if(player.Pet == null)
            {
                player.SendError("You don't have any pet at this moment!");
            }
        }
    }
	
    internal class WhoCommand : ICommand
    {
        public string Command
        {
            get { return "who"; }
        }

        public int RequiredRank
        {
            get { return 0; }
        }

        public void Execute(Player player, string[] args)
        {
            var sb = new StringBuilder("Players online: ");
            var copy = player.Owner.Players.Values.ToArray();
            for (var i = 0; i < copy.Length; i++)
            {
                if (i != 0) sb.Append(" - ");
                sb.Append(copy[i].Name);
            }

            player.SendInfo(sb.ToString());
        }
    }

    internal class ServerCommand : ICommand
    {
        public string Command
        {
            get { return "server"; }
        }

        public int RequiredRank
        {
            get { return 0; }
        }

        public void Execute(Player player, string[] args)
        {
            player.SendInfo(player.Owner.Name);
        }
    }

    /*internal class PauseCommand : ICommand
    {
        public string Command
        {
            get { return "pause"; }
        }

        public int RequiredRank
        {
            get { return 0; }
        }

        public void Execute(Player player, string[] args)
        {
            if (player.Owner.Name != "Battle Arena" && player.Owner.Name != "Free Battle Arena")
            {
                if (player.HasConditionEffect(ConditionEffects.Paused))
                {
                    player.ApplyConditionEffect(new ConditionEffect
                    {
                        Effect = ConditionEffectIndex.Paused,
                        DurationMS = 0
                    });
                    player.SendInfo("Game resumed.");
                }
                else
                {
                    foreach (var i in player.Owner.EnemiesCollision.HitTest(player.X, player.Y, 6).OfType<Enemy>())
                    {
                        if (i.ObjectDesc.Enemy)
                        {
                            player.SendInfo("Not safe to pause.");
                            return;
                        }
                    }
                    player.ApplyConditionEffect(new ConditionEffect
                    {
                        Effect = ConditionEffectIndex.Paused,
                        DurationMS = -1
                    });
                    player.SendInfo("Game paused.");
                }
            }
            else
            {
                player.SendError("You cannot pause in the arena");
            }
        }
    }*/

    /*internal class TeleportCommand : ICommand
    {
        public string Command
        {
            get { return "teleport"; }
        }

        public int RequiredRank
        {
            get { return 0; }
        }

        public void Execute(Player player, string[] args)
        {
            try
            {
                if (player.nName.ToLower() == args[0].ToLower())
                {
                    player.SendInfo("You are already at yourself, and always will be!");
                    return;
                }
                if (player.Owner.AllowTeleport == false)
                {
                    player.SendInfo("You are not allowed to teleport in this area!");
                    return;
                }
                foreach (var i in player.Owner.Players)
                {
                    if (i.Value.nName.ToLower() == args[0].ToLower().Trim())
                    {
                        if (i.Value.HasConditionEffect(ConditionEffects.Invisible))
                        {
                            player.SendInfo("Could not teleport to this player.");
                            return;
                        }
                        player.Teleport(new RealmTime(), new TeleportPacket
                        {
                            ObjectId = i.Value.Id
                        });
                        return;
                    }
                }
                player.SendInfo(string.Format("Cannot teleport, {0} not found!", args[0].Trim()));
            }
            catch
            {
                player.SendHelp("Usage: /teleport <player name>");
            }
        }
    }*/

    internal class TeleportCommand : ICommand
    {
        public string Command
        {
            get { return "teleport"; }
        }

        public int RequiredRank
        {
            get { return 0; }
        }

        public void Execute(Player player, string[] args)
        {
            try
            {
                if (player.nName.ToLower() == args[0].ToLower())
                {
                    player.SendInfo("You are already at yourself, and always will be!");
                    return;
                }
                if (player.Owner.AllowTeleport == false)
                {
                    player.SendInfo("You are not allowed to teleport in this area!");
                    return;
                }
                foreach (var i in player.Owner.Players)
                {
                    if (i.Value.nName.ToLower() == args[0].ToLower().Trim())
                    {
                        if (i.Value.HasConditionEffect(ConditionEffects.Invisible))
                        {
                            player.SendInfo("Could not teleport to this player.");
                            return;
                        }
                        player.Teleport(new RealmTime(), new TeleportPacket
                        {
                            ObjectId = i.Value.Id
                        }
                        );
                        float posx, posy;
                        posx = player.X;
                        posy = player.Y;
                        if (player.Pet != null)
                        {
                            int ipsilon;
                            for (ipsilon = 0; ipsilon < 5; ipsilon++)
                            {
                                player.Pet.Move(posx, posy);
                            }
                            return;
                        }
                        /*if (player.Pet != null)
                            player.Pet.Move(posx, posy);
                        return;*/
                    }
                }
                player.SendInfo(string.Format("Cannot teleport, {0} not found!", args[0].Trim()));
            }
            catch
            {
                player.SendHelp("Usage: /teleport <player name>");
            }
        }
    }
	
    internal class TellCommand : ICommand
    {
        public string Command
        {
            get { return "tell"; }
        }

        public int RequiredRank
        {
            get { return 0; }
        }

        public void Execute(Player player, string[] args)
        {
            try
            {
                var sindex = 1;

                if (!(player.NameChosen))
                {
                    player.SendInfo(string.Format("Choose a name!"));
                    return;
                }

                List<string> tags = new List<string>();
                foreach (var x in RealmManager.Worlds)
                {
                    foreach (var y in x.Value.Players)
                    {
                        if (y.Value.Client.Account.Tag != "" && y.Value.Client.Account.Tag != null)
                        {
                            tags.Add("(" + y.Value.Client.Account.Tag + ")");
                        }
                    }
                }
                var playername = args[0].Trim();
                if (tags.Contains(playername))
                {
                    playername = args[1];
                    sindex = 2;
                }

                if (player.nName.ToLower() == playername.ToLower())
                {
                    player.SendInfo(string.Format("You canot tell yourself!"));
                    return;
                }

                var saytext = string.Join(" ", args, sindex, args.Length - sindex);

                foreach (var w in RealmManager.Worlds)
                {
                    var world = w.Value;
                    if (w.Key != 0) // 0 is limbo??
                    {
                        foreach (var i in world.Players)
                        {
                            if (i.Value.nName.ToLower() == playername.ToLower().Trim() && i.Value.NameChosen)
                            {
                                if (saytext == "" || saytext == null)
                                {
                                    player.SendHelp("Usage: /tell <player name> <text>");
                                    return;
                                }
                                else
                                {
                                    player.Client.SendPacket(new TextPacket //echo to self
                                    {
                                        BubbleTime = 10,
                                        Stars = player.Stars,
                                        Name = player.Name,
                                        Recipient = i.Value.Name,
                                        ObjectId = player.Id,
                                        Text = saytext.ToSafeText()
                                    });

                                    i.Value.Client.SendPacket(new TextPacket //echo to /tell player
                                    {
                                        BubbleTime = 10,
                                        Stars = player.Stars,
                                        Recipient = i.Value.nName,
                                        Name = player.Name,
                                        ObjectId = (i.Value.Owner.Id == player.Owner.Id ? player.Id : 0),
                                        Text = saytext.ToSafeText()
                                    });
                                    return;
                                }
                            }
                        }
                    }
                }
                player.SendInfo(string.Format("Cannot tell, {0} not found!", args[sindex - 1].Trim()));
            }
            catch
            {
                player.SendInfo("Cannot tell!");
            }
        }
    }

    internal class DyeCommand : ICommand
    {
        public string Command
        {
            get { return "dye"; }
        }

        public int RequiredRank
        {
            get { return 4; }
        }

        public void Execute(Player player, string[] args)
        {
            var name = string.Join(" ", args.ToArray()).Trim();
            short objType;
            if (!XmlDatas.IdToType.TryGetValue(name, out objType))
            {
                player.SendInfo("Unknown dye!");
                return;
            }
            try
            {
                if (XmlDatas.TypeToElement[objType].Element("Class").Value == "Dye")
                {
                    for (var i = 0; i < player.Inventory.Length; i++)
                        if (player.Inventory[i] == null)
                        {
                            player.Inventory[i] = XmlDatas.ItemDescs[objType];
                            player.UpdateCount++;
                            return;
                        }
                }
                else
                {
                    player.SendInfo("Unknown dye!");
                    return;
                }
            }
            catch
            {
                return;
            }
        }
    }

    /*internal class VisitCommand : ICommand
    {
        public string Command
        {
            get { return "rec"; }
        }

        public int RequiredRank
        {
            get { return 11; }
        }

        public void Execute(Player player, string[] args)
        {
            var name = string.Join(" ", args.ToArray()).Trim();
            try
            {
                var PlayerData = PlayerDataList.GetData(player.Client.Account.Name);
                foreach (var w in RealmManager.Worlds)
                {
                    var world = w.Value;
                    if (w.Key != 0)
                    {
                        foreach (var i in world.Players)
                        {
                            if (i.Value.Client.Account.Name.ToLower() == name.ToLower())
                            {
                                var iPlayerData = PlayerDataList.GetData(i.Value.Client.Account.Name);
                                if (!(player.Client.Account.Rank > 2))
                                {
                                    if (world.Name != "Vault")
                                    {
                                        if (world.Name != "Guild Hall")
                                        {
                                            TryJoin(player, iPlayerData, world, i.Value);
                                            return;
                                        }
                                        else
                                        {
                                            if ((world as GuildHall).Guild == player.Guild)
                                            {
                                                TryJoin(player, iPlayerData, world, i.Value);
                                                return;
                                            }
                                            else
                                            {
                                                player.SendInfo("Player is in " + i.Value.Guild + "'s guild hall!");
                                                return;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (world.Name == "Vault")
                                        {
                                            player.SendInfo("Player is in Vault!");
                                            return;
                                        }
                                        else if (world.Name == "Guild Hall")
                                        {
                                            player.SendInfo("Player is in Guild Hall!");
                                            return;
                                        }
                                        else
                                        {
                                            if (!iPlayerData.UsingGroup)
                                            {
                                                player.Client.Reconnect(new ReconnectPacket
                                                {
                                                    Host = "",
                                                    Port = 2050,
                                                    GameId = world.Id,
                                                    Name = i.Value.Name + "'s Vault",
                                                    Key = Empty<byte>.Array,
                                                });
                                                return;
                                            }
                                            else
                                            {
                                                foreach (var o in iPlayerData.JGroup)
                                                {
                                                    if (o.Value == player.Client.Account.Name.ToLower())
                                                    {
                                                        player.Client.Reconnect(new ReconnectPacket
                                                        {
                                                            Host = "",
                                                            Port = 2050,
                                                            GameId = world.Id,
                                                            Name = i.Value.Name + "'s Vault",
                                                            Key = Empty<byte>.Array,
                                                        });
                                                        return;
                                                    }
                                                }
                                                player.SendInfo("Not in " + i.Value.Client.Account.Name + "'s group!");
                                                return;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    player.Client.Reconnect(new ReconnectPacket
                                    {
                                        Host = "",
                                        Port = 2050,
                                        GameId = world.Id,
                                        Name = i.Value.Owner.Name,
                                        Key = Empty<byte>.Array,
                                    });
                                    return;
                                }
                            }
                        }
                    }
                }
                player.SendHelp("Use /visit <playername>");
            }
            catch
            {
                player.SendInfo("Unexpected error in command!");
            }
        }

        public bool TryJoin(Player player, GlobalPlayerData iPlayerData, World world, Player i)
        {
            if (world.Id == -60)
            {
                player.SendInfo("Sorry but that user is in a restricted area!");
                return true;
            }
            if (world.Name == "Admin Room")
            {
                if (!iPlayerData.Solo || player.Client.Account.Rank > 3)
                {
                    if (!iPlayerData.UsingGroup)
                    {
                        player.Client.Reconnect(new ReconnectPacket
                        {
                            Host = "",
                            Port = 2050,
                            GameId = world.Id,
                            Name = world.Name,
                            Key = Empty<byte>.Array,
                        });
                        return true;
                    }
                    else
                    {
                        foreach (var o in iPlayerData.JGroup)
                        {
                            if (o.Value == player.Client.Account.Name.ToLower())
                            {
                                player.Client.Reconnect(new ReconnectPacket
                                {
                                    Host = "",
                                    Port = 2050,
                                    GameId = world.Id,
                                    Name = world.Name,
                                    Key = Empty<byte>.Array,
                                });
                                return true;
                            }
                        }
                        player.SendInfo("Not in " + i.Client.Account.Name + "'s group!");
                        return true;
                    }
                }
                else
                {
                    player.SendInfo("Player is going solo!");
                    return true;
                }
            }
            else
            {
                player.SendInfo("Sorry but that user is in a restricted area!");
                return true;
            }
        }
    }*/

    internal class VisitCommand : ICommand
    {
        public string Command
        {
            get { return "rec"; }
        }

        public int RequiredRank
        {
            get { return 11; }
        }

        public void Execute(Player player, string[] args)
        {
            var name = string.Join(" ", args.ToArray()).Trim();
            try
            {
                var PlayerData = PlayerDataList.GetData(player.Client.Account.Name);
                foreach (var w in RealmManager.Worlds)
                {
                    var world = w.Value;
                    if (w.Key != 0)
                    {
                        foreach (var i in world.Players)
                        {
                            if (i.Value.Client.Account.Name.ToLower() == name.ToLower())
                            {
                                var iPlayerData = PlayerDataList.GetData(i.Value.Client.Account.Name);
                                //if (!(player.Client.Account.Rank > 2))
                                if((player.Client.Account.Rank >= 11) && (world.Name != "Vault") && (world.Name != "Guild Hall"))
                                {
                                    /*if (world.Name != "Vault")
                                    {
                                        if (world.Name != "Guild Hall")
                                        {
                                            TryJoin(player, iPlayerData, world, i.Value);
                                            return;
                                        }
                                        else
                                        {
                                            if ((world as GuildHall).Guild == player.Guild)
                                            {
                                                TryJoin(player, iPlayerData, world, i.Value);
                                                return;
                                            }
                                            else
                                            {
                                                player.SendInfo("Player is in " + i.Value.Guild + "'s guild hall!");
                                                return;
                                            }
                                        }
                                    }*/
                                    if((world.Name != "Vault") || (world.Name != "Guild Hall"))
                                    {
                                        player.Client.Reconnect(new ReconnectPacket
                                        {
                                            Host = "",
                                            Port = 2050,
                                            GameId = world.Id,
                                            Name = i.Value.Owner.Name,
                                            Key = Empty<byte>.Array,
                                        });
                                        return;
                                    }
                                    /*else
                                    {
                                        if (world.Name == "Vault")
                                        {
                                            player.SendError("Player is in owns Vault! You can't connect while is over there.");
                                            return;
                                        }
                                        else if (world.Name == "Guild Hall")
                                        {
                                            player.SendError("Player is in Guild Hall! You can't connect direct to player Guild Hall with this command, go to Nexus first to access his Guild.");
                                            return;
                                        }
                                        /*else
                                        {
                                            if (!iPlayerData.UsingGroup)
                                            {
                                                player.Client.Reconnect(new ReconnectPacket
                                                {
                                                    Host = "",
                                                    Port = 2050,
                                                    GameId = world.Id,
                                                    Name = i.Value.Name + "'s Vault",
                                                    Key = Empty<byte>.Array,
                                                });
                                                return;
                                            }
                                            else
                                            {
                                                foreach (var o in iPlayerData.JGroup)
                                                {
                                                    if (o.Value == player.Client.Account.Name.ToLower())
                                                    {
                                                        player.Client.Reconnect(new ReconnectPacket
                                                        {
                                                            Host = "",
                                                            Port = 2050,
                                                            GameId = world.Id,
                                                            Name = i.Value.Name + "'s Vault",
                                                            Key = Empty<byte>.Array,
                                                        });
                                                        return;
                                                    }
                                                }
                                                player.SendInfo("Not in " + i.Value.Client.Account.Name + "'s group!");
                                                return;
                                            }
                                        }
                                    }*/
                                }
                                else if(player.Client.Account.Rank == 11)
                                {
                                    player.Client.Reconnect(new ReconnectPacket
                                    {
                                        Host = "",
                                        Port = 2050,
                                        GameId = world.Id,
                                        Name = i.Value.Owner.Name,
                                        Key = Empty<byte>.Array,
                                    });
                                    return;
                                }
                            }
                        }
                    }
                }
                player.SendHelp("Use /rec <player name>");
            }
            catch
            {
                player.SendInfo("Unexpected error in command!");
            }
        }

        public bool TryJoin(Player player, GlobalPlayerData iPlayerData, World world, Player i)
        {
            if (world.Name == "Tournament Arena")
            {
                player.SendInfo("Sorry but that user is in Tournament Arena!");
                return true;
            }
            if (world.Name == "Tournament Arena")
            {
                if (!iPlayerData.Solo || player.Client.Account.Rank > 11)
                {
                    if (!iPlayerData.UsingGroup)
                    {
                        player.Client.Reconnect(new ReconnectPacket
                        {
                            Host = "",
                            Port = 2050,
                            GameId = world.Id,
                            Name = world.Name,
                            Key = Empty<byte>.Array,
                        });
                        return true;
                    }
                    else
                    {
                        foreach (var o in iPlayerData.JGroup)
                        {
                            if (o.Value == player.Client.Account.Name.ToLower())
                            {
                                player.Client.Reconnect(new ReconnectPacket
                                {
                                    Host = "",
                                    Port = 2050,
                                    GameId = world.Id,
                                    Name = world.Name,
                                    Key = Empty<byte>.Array,
                                });
                                return true;
                            }
                        }
                        player.SendInfo("Not in " + i.Client.Account.Name + "'s group!");
                        return true;
                    }
                }
                else
                {
                    player.SendInfo("Player is going solo!");
                    return true;
                }
            }
            else
            {
                player.SendInfo("Sorry but that user is in a restricted area!");
                return true;
            }
        }
    }
	
    internal class GroupCommand : ICommand
    {
        public string Command
        {
            get { return "group"; }
        }

        public int RequiredRank
        {
            get { return 0; }
        }

        public void Execute(Player player, string[] args)
        {
            try
            {
                var PlayerData = PlayerDataList.GetData(player.Client.Account.Name);
                if (args.Length > 0)
                {
                    var subcommand = args[0];
                    if (subcommand == "list")
                    {
                        var glist = "Players in your group: ";
                        foreach (var i in PlayerData.JGroup)
                        {
                            if (glist != "Players in your group: ")
                            {
                                glist = glist + ", " + i;
                            }
                            else
                            {
                                glist = glist + i;
                            }
                        }
                        player.SendInfo(glist);
                    }
                    else if (subcommand == "add" && args.Length > 1)
                    {
                        foreach (var i in PlayerData.JGroup)
                        {
                            if (i.Value == args[1].ToLower())
                            {
                                player.SendInfo("Player already added!");
                                return;
                            }
                        }
                        PlayerData.JGroup.TryAdd(PlayerData.JGroup.Count, args[1].ToLower());
                        player.SendInfo("Added " + args[1] + "!");
                    }
                    else if (subcommand == "del" && args.Length > 1)
                    {
                        var remc = 0;
                        foreach (var i in PlayerData.JGroup)
                        {
                            if (i.Value == args[1].ToLower())
                            {
                                string absolutelynothingdisregardthis;
                                player.SendInfo("Removed player " + i.Value + "!");
                                remc++;
                                PlayerData.JGroup.TryRemove(i.Key, out absolutelynothingdisregardthis);
                            }
                        }
                        if (remc < 1)
                        {
                            player.SendInfo("Player not found!");
                        }
                    }
                }
                else
                {
                    if (PlayerData.UsingGroup)
                    {
                        PlayerData.UsingGroup = false;
                        player.SendInfo("Group-only join disabled!");
                    }
                    else
                    {
                        PlayerData.UsingGroup = true;
                        player.SendInfo("Group-only join enabled!");
                    }
                }
            }
            catch
            {
                player.SendInfo("Unexpected error in command!");
            }
        }
    }

    /*internal class SoloCommand : ICommand
    {
        public string Command
        {
            get { return "solo"; }
        }

        public int RequiredRank
        {
            get { return 3; }
        }

        public void Execute(Player player, string[] args)
        {
            var PlayerData = PlayerDataList.GetData(player.Client.Account.Name);
            if (PlayerData.Solo)
            {
                PlayerData.Solo = false;
                player.SendInfo("Solo disabled! People can now join you!");
            }
            else
            {
                PlayerData.Solo = true;
                player.SendInfo("Solo enabled! People can no longer join you!");
            }
        }
    }*/

    internal class UpdateList : ICommand
    {
        public string Command
        {
            get { return "update"; }
        }

        public int RequiredRank
        {
            get { return 0; }
        }

        public void Execute(Player player, string[] args)
        {
            player.Client.SendPacket(new TextBoxPacket
            {
                Title = "Latest News! Update v27.2.95:",
                Message = "* Security improved for community;"+
                        "\n* Fixed teleport dupeing bug;"+
                        "\n* Added more commands for Moderators;"+
                        "\n* L.S.S.S. updated to version 3.0;"+
                        "\n* New realm events coming..."+
                        "\n\nRemember to report any power abuse by staff member. Insulting, external programs to change gameplay, scamming, dupeing and something like it can result in banishment or IP block to access server! To make banishment faster and erradicate incorrect players make video proofing your report."+
                        "\nBest Regards, owner Devwarlt and LoE Team.",
                Button1 = "Alright!"
            });
        }
    }
	
    /*internal class ShopCommand : ICommand
    {
        public string Command
        {
            get { return "shop"; }
        }

        public int RequiredRank
        {
            get { return 3; }
        }

        public void Execute(Player player, string[] args)
        {
            var evar = string.Join(" ", args.ToArray()).Trim();
            if (args.Length > 0)
            {
                var shop = RealmManager.AddWorld(new ShopMap(""));
                RealmManager.ShopWorlds.TryGetValue(evar, out shop);
                player.Client.Reconnect(new ReconnectPacket
                {
                    Host = "",
                    Port = 2050,
                    GameId = shop.Id,
                    Name = "Shop",
                    Key = Empty<byte>.Array
                });
            }
            else
            {
                var shopnames = "";
                var tname = "";
                foreach (var i in MerchantLists.shopLists)
                {
                    if (shopnames == "")
                    {
                        tname = i.Key;
                        tname.Insert(0, tname[0].ToString().ToUpper());
                        shopnames = i.Key;
                    }
                    else
                    {
                        tname = i.Key;
                        tname.Insert(0, tname[0].ToString().ToUpper());
                        shopnames += ", " + i.Key;
                    }
                }
                player.SendInfo("Shops: " + shopnames);
            }
        }
    }*/

    internal class ListCommandsVIP : ICommand
    {
        public string Command
        {
            get { return "vipcommands"; }
        }

        public int RequiredRank
        {
            get { return 3; }
        }

        public void Execute(Player player, string[] args)
        {
            player.Client.SendPacket(new TextBoxPacket
            {
                Title = "Commands for VIPs!",
                Message = "1) /vip  Chat for VIPs only."+
                        "\n2) /nexusvip - Access instantly in Nexus VIP."+
                        "\n3) /rec <player> - Reconnect in same place where your friend are."+
                        "\n4) /ip <player> - Verify the IP Address of any player online."+
                        "\n\nNOTE: /rec and /ip command has been disabled after Vault invasion by strange players and security.",
                Button1 = "Got It!"
            });
        }
    }
	
    internal class ListCommands : ICommand
    {
        public string Command
        {
            get { return "commands"; }
        }

        public int RequiredRank
        {
            get { return 0; }
        }

        public void Execute(Player player, string[] args)
        {
            var cmds = new Dictionary<string, ICommand>();
            var t = typeof (ICommand);
            foreach (var i in t.Assembly.GetTypes())
                if (t.IsAssignableFrom(i) && i != t)
                {
                    var instance = (ICommand) Activator.CreateInstance(i);
                    cmds.Add(instance.Command, instance);
                }
            var sb = new StringBuilder("");
            var copy = cmds.Values.ToArray();
            for (var i = 0; i < copy.Length; i++)
            {
                if (i != 0) sb.Append("  |  ");
                sb.Append(copy[i].Command);
            }

            player.Client.SendPacket(new TextBoxPacket
            {
                Title = "Commands:",
                Message = (sb.ToString()),
                Button1 = "Ok"
            });
        }
    }

    //class ListCommands : ICommand
    //{
    //    public string Command { get { return "commands"; } }
    //    public int RequiredRank { get { return 0; } }

    //    public void Execute(Player player, string[] args)
    //    {
    //        Dictionary<string, ICommand> cmds = new Dictionary<string, ICommand>();
    //        var t = typeof(ICommand);
    //        foreach (var i in t.Assembly.GetTypes())
    //            if (t.IsAssignableFrom(i) && i != t)
    //            {
    //                var instance = (ICommand)Activator.CreateInstance(i);
    //                cmds.Add(instance.Command, instance);
    //            }
    //        StringBuilder sb = new StringBuilder("Commands: ");
    //        var copy = cmds.Values.ToArray();
    //        for (int i = 0; i < copy.Length; i++)
    //        {
    //            if (i != 0) sb.Append(", ");
    //            sb.Append(copy[i].Command);
    //        }

    //        player.SendInfo(sb.ToString());
    //    }
    //}


    internal class statsCommand : ICommand
    {
        public string Command
        {
            get { return "mystats"; }
        }

        public int RequiredRank
        {
            get { return 0; }
        }

        public void Execute(Player player, string[] args)
        {
            foreach (var i in RealmManager.Clients.Values)
                i.SendPacket(new NotificationPacket
                {
                    Color = new ARGB(0xff00ff00),
                    ObjectId = player.Id,
                    Text = "HP:" + player.HP + " " + "MP:" + player.MP + " " + "Fame:" + " " + player.Fame
                });
            player.SendInfo("HP:" + player.HP + " " + "MP:" + player.MP + " " + "Att:" + " " + player.Stats[2] + " " +
                            "Def:" + " " + player.Stats[3] + " " + "Spd:" + " " + player.Stats[4] + " " + "Vit:" + " " +
                            player.Stats[5] + " " + "Wis:" + " " + player.Stats[6] + " " + "Dex:" + " " +
                            player.Stats[7]);
        }
    }

    internal class sayCommand : ICommand
    {
        public string Command
        {
            get { return "say"; }
        }

        public int RequiredRank
        {
            get { return 4; }
        }

        public void Execute(Player player, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /say <saytext>");
            }
            else
            {
                var saytext = string.Join(" ", args);
                foreach (var i in RealmManager.Clients.Values)
                    i.SendPacket(new NotificationPacket
                    {
                        Color = new ARGB(0xff00ff00),
                        ObjectId = player.Id,
                        Text = saytext
                    });
            }
        }
    }

    internal class FameBasCommand : ICommand
    {
        public string Command
        {
            get { return "famebase"; }
        }

        public int RequiredRank
        {
            get { return 0; }
        }

        public void Execute(Player player, string[] args)
        {
            foreach (var i in RealmManager.Clients.Values)
                i.SendPacket(new NotificationPacket
                {
                    Color = new ARGB(0xff00ff00),
                    ObjectId = player.Id,
                    Text = "Fame base:" + " " + player.Fame
                });
            player.SendInfo("Fame base:" + " " + player.Fame);
        }
    }
    
	internal class AFKCommand : ICommand
    {
        public string Command
        {
            get { return "afk"; }
        }

        public int RequiredRank
        {
            get { return 0; }
        }

        public void Execute(Player player, string[] args)
        {
            if (player.Owner.Name != "Battle Arena" && player.Owner.Name != "Free Battle Arena" && player.Owner.Name != "Tournament Arena")
            {
                if (player.HasConditionEffect(ConditionEffects.Paused))
                {
                    foreach (var i in RealmManager.Clients.Values)
                        i.SendPacket(new NotificationPacket
                        {
                            Color = new ARGB(0xff00ff00),
                            ObjectId = player.Id,
                            Text = "Active"
                        });
                    player.ApplyConditionEffect(new ConditionEffect
                    {
                        Effect = ConditionEffectIndex.Paused,
                        DurationMS = 0
                    });
                    player.SendInfo("Active!");
                }
                else
                {
                    foreach (var i in player.Owner.EnemiesCollision.HitTest(player.X, player.Y, 8).OfType<Enemy>())
                    {
                        if (i.ObjectDesc.Enemy)
                        {
                            player.SendInfo("Not safe to go AFK.");
                            return;
                        }
                    }
                    foreach (var i in RealmManager.Clients.Values)
                        i.SendPacket(new NotificationPacket
                        {
                            Color = new ARGB(0xff00ff00),
                            ObjectId = player.Id,
                            Text = "AFK"
                        });
                    player.ApplyConditionEffect(new ConditionEffect
                    {
                        Effect = ConditionEffectIndex.Paused,
                        DurationMS = -1
                    });
                    player.SendInfo("AFK!");
                }
            }
            else
            {
                player.SendInfo("You cannot pause in the arena");
            }
        }
    }
	
	internal class PauseCommand : ICommand
    {
        public string Command
        {
            get { return "pause"; }
        }

        public int RequiredRank
        {
            get { return 0; }
        }

        public void Execute(Player player, string[] args)
        {
            if (player.Owner.Name != "Battle Arena" && player.Owner.Name != "Free Battle Arena" && player.Owner.Name != "Tournament Arena")
            {
                if (player.HasConditionEffect(ConditionEffects.Paused))
                {
                    foreach (var i in RealmManager.Clients.Values)
                        i.SendPacket(new NotificationPacket
                        {
                            Color = new ARGB(0xff00ff00),
                            ObjectId = player.Id,
                            Text = "Active"
                        });
                    player.ApplyConditionEffect(new ConditionEffect
                    {
                        Effect = ConditionEffectIndex.Paused,
                        DurationMS = 0
                    });
                    player.SendInfo("Active!");
                }
                else
                {
                    foreach (var i in player.Owner.EnemiesCollision.HitTest(player.X, player.Y, 8).OfType<Enemy>())
                    {
                        if (i.ObjectDesc.Enemy)
                        {
                            player.SendInfo("Not safe to go AFK.");
                            return;
                        }
                    }
                    foreach (var i in RealmManager.Clients.Values)
                        i.SendPacket(new NotificationPacket
                        {
                            Color = new ARGB(0xff00ff00),
                            ObjectId = player.Id,
                            Text = "AFK"
                        });
                    player.ApplyConditionEffect(new ConditionEffect
                    {
                        Effect = ConditionEffectIndex.Paused,
                        DurationMS = -1
                    });
                    player.SendInfo("AFK!");
                }
            }
            else
            {
                player.SendInfo("You cannot pause in the arena");
            }
        }
    }

    internal class ArenasCommand : ICommand
    {
        public string Command
        {
            get { return "arenas"; }
        }

        public int RequiredRank
        {
            get { return 0; }
        }

        public void Execute(Player player, string[] args)
        {
            var Arenas = new List<BattleArenaMap>();
            var ArenaTexts = new List<string>();

            ArenaTexts.Add("");

            foreach (var w in RealmManager.Worlds)
            {
                var world = w.Value;
                if (w.Value.Name == "Battle Arena" && w.Value.Players.Count > 0 ||
                    w.Value.Name == "Free Battle Arena" && w.Value.Players.Count > 0)
                {
                    Arenas.Add(w.Value as BattleArenaMap);
                }
            }
            if (Arenas.Count > 0)
            {
                foreach (var w in Arenas)
                {
                    var ctext = "Wave " + w.Wave + " - {0} {1}";
                    var players = new List<string>();
                    var solo = 0;
                    foreach (var p in w.Players)
                    {
                        players.Add(p.Value.Name);
                        if (PlayerDataList.GetData(p.Value.Client.Account.Name).Solo)
                            solo++;
                    }
                    if (players.Count > 0)
                    {
                        ArenaTexts.Add(string.Format(ctext, string.Join(", ", players.ToArray()),
                            solo == players.Count ? " (SOLO)" : ""));
                    }
                }
            }

            if (ArenaTexts.Count == 1)
                ArenaTexts.Add("None");

            //player.SendInfo(string.Join("\n", ArenaTexts.ToArray()));

            player.Client.SendPacket(new TextBoxPacket
            {
                Title = "Current Arenas:",
                Message = string.Join("\n", ArenaTexts.ToArray()),
                Button1 = "Ok"
            });
        }
    }

    //class ArenasCommand : ICommand 
    //{
    //    public string Command { get { return "arenas"; } }
    //    public int RequiredRank { get { return 0; } }

    //    public void Execute(Player player, string[] args)
    //    {
    //        List<BattleArenaMap> Arenas = new List<BattleArenaMap>();
    //        List<string> ArenaTexts = new List<string>();

    //        ArenaTexts.Add("Current arenas:");

    //        foreach (var w in RealmManager.Worlds)
    //        {
    //            World world = w.Value;
    //            if (w.Value.Name == "Battle Arena" && w.Value.Players.Count > 0)
    //            {
    //                Arenas.Add(w.Value as BattleArenaMap);
    //            }
    //        }
    //        if (Arenas.Count > 0)
    //        {
    //            foreach (var w in Arenas)
    //            {
    //                string ctext = "Wave "+ w.Wave +" - {0} {1}";
    //                List<string> players = new List<string>();
    //                int solo = 0;
    //                foreach (var p in w.Players)
    //                {
    //                    players.Add(p.Value.Name);
    //                    if (PlayerDataList.GetData(p.Value.Client.Account.Name).Solo)
    //                        solo++;
    //                }
    //                if (players.Count > 0)
    //                {
    //                    ArenaTexts.Add(string.Format(ctext, string.Join(", ", players.ToArray()), solo == players.Count ? " (SOLO)" : ""));
    //                }
    //            }
    //        }

    //        if(ArenaTexts.Count == 1)
    //            ArenaTexts.Add("None");

    //        player.SendInfo(string.Join("\n", ArenaTexts.ToArray()));
    //    }
    //}

    internal class LeaderboardCommand : ICommand
    {
        public string Command
        {
            get { return "toparena"; }
        }

        public int RequiredRank
        {
            get { return 0; }
        }

        public void Execute(Player player, string[] args)
        {
            var leaderboardInfo = new Database().GetArenaLeaderboards();
           
            player.Client.SendPacket(new TextBoxPacket
            {
                Title = "TOP Arena:",
                Message = string.Join("\n", leaderboardInfo),
                Button1 = "Ok"
            });
           
        }
    }

    internal class GuildLeaderboardCommand : ICommand
    {
        public string Command
        {
            get { return "topguild"; }
        }

        public int RequiredRank
        {
            get { return 0; }
        }

        public void Execute(Player player, string[] args)
        {
            var leaderboardInfo = new Database().GetGuildLeaderboards();
            
            player.Client.SendPacket(new TextBoxPacket
            {
                Title = "TOP Guild Fame:",
                Message = string.Join("\n", leaderboardInfo),
                Button1 = "Ok"
            });
            
        }
    }

    /*internal class SellCommand : ICommand
    {
        public string Command
        {
            get { return "sell"; }
        }

        public int RequiredRank
        {
            get { return 3; }
        }

        public void Execute(Player player, string[] args)
        {
            try
            {
                player.Decision = 0;
                player.price = new Prices();
                var slotList = new List<int>();
                var slotList2 = new List<int>();
                for (var i = 0; i < args.Length; i++)
                    if (!slotList.Contains(Convert.ToInt32(args[i])))
                        slotList.Add(Convert.ToInt32(args[i]));
                if (slotList.Count < 1)
                    throw new Exception();
                foreach (var i in slotList)
                    if (!(i < 0) && !(i > 8))
                    {
                        var realslot = i + 3;
                        if (player.Inventory[realslot] != null)
                        {
                            slotList2.Add(realslot);
                        }
                    }
                player.price.SellSlots = slotList2;
                if (!player.price.HasPrices(player))
                {
                    player.SendInfo("No prices for specified items!");
                }
                else
                {
                    var msgSlots = new List<int>();
                    foreach (var i in player.price.SellSlots)
                        try
                        {
                            msgSlots.Add(i - 3);
                        }
                        catch
                        {
                        }
                    player.SendInfo("Slots being sold: [" + string.Join(", ", msgSlots) + "]");
                    player.SendInfo("You gain " + player.price.GetPrices(player) +
                                    " fame from these items. Sell them?\nType /yes or /no");
                    player.Decision = 2;
                }
            }
            catch
            {
                player.SendHelp("Usage: /sell <slot #1> <slot #2> etc.");
            }
        }
    }*/

    //class ForgeListCommand : ICommand
    //{
    //    public string Command { get { return "forgelist"; } }
    //    public int RequiredRank { get { return 0; } }

    //    public void Execute(Player player, string[] args)
    //    {
    //        List<string> itemNames = new List<string>();
    //        Combinations c = new Combinations();
    //        foreach (var i in c.combos)
    //        {
    //            itemNames.Add(i.Value.Item1);
    //        }
    //        player.SendInfo("These are the current items that can be forged:\n" + string.Join(", ", itemNames.ToArray()));
    //    }
    //}

    internal class PremChat : ICommand
    {
        public string Command
        {
            get { return "vip"; }
        }

        public int RequiredRank
        {
            get { return 3; }
        }

        public void Execute(Player player, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Use /vip <text>");
            }

            else if (player.Client.Account.Rank >= 2)
            {
                try
                {
                    var saytext = string.Join(" ", args);

                    foreach (var w in RealmManager.Worlds)
                    {
                        var world = w.Value;
                        if (w.Key != 0)
                        {
                            foreach (var i in world.Players)
                            {
                                if (i.Value.Client.Account.Rank >= 3)
                                {
                                    i.Value.Client.SendPacket(new TextPacket
                                    {
                                        BubbleTime = 10,
                                        ObjectId = player.Id,
                                        Stars = player.Stars,
                                        Name = "#(VIP) " + player.nName,
                                        Text = " " + saytext
                                    });
                                }
                            }
                        }
                    }
                }
                catch
                {
                    player.SendInfo("Cannot VIP chat!");
                }
            }
            else
                player.SendInfo("You need to be a VIP to use this command.");
        }
    }

    internal class GlobalChat : ICommand
    {
        public string Command
        {
            get { return "global"; }
        }

        public int RequiredRank
        {
            get { return 0; }
        }

        public void Execute(Player player, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Use /global <text>");
            }

            else if (player.Client.Account.Rank >= 0)
            {
                try
                {
                    var saytext = string.Join(" ", args);

                    foreach (var w in RealmManager.Worlds)
                    {
                        var world = w.Value;
                        if (w.Key != 0)
                        {
                            foreach (var i in world.Players)
                            {
                                if (i.Value.Client.Account.Rank >= 0)
                                {
                                    i.Value.Client.SendPacket(new TextPacket
                                    {
                                        BubbleTime = 10,
                                        ObjectId = player.Id,
                                        Stars = player.Stars,
                                        Name = "#(Global Chat) " + player.Name,
                                        Text = " " + saytext
                                    });
                                }
                            }
                        }
                    }
                }
                catch
                {
                    player.SendInfo("Cannot global chat!");
                }
            }
        }
    }
	
    internal class PackCommand : ICommand
    {
        public string Command
        {
            get { return "b"; }
        }

        public int RequiredRank
        {
            get { return 0; }
        }

        public void Execute(Player player, string[] args)
        {
            if (player.tradeTarget == null)
            {
                if (player.SwapBackpack(Convert.ToInt32(args[0])))
                {
                    player.SendInfo("Switched to backpack #" + args[0]);
                }
                else
                {
                    player.SendInfo("Backpack #" + args[0] + " does not exist!");
                }
            }
            else
            {
                player.SendError("Cannot switch backpack while trading!");
            }
        }
    }

    internal class Pack2Command : ICommand
    {
        public string Command
        {
            get { return "bp"; }
        }

        public int RequiredRank
        {
            get { return 0; }
        }

        public void Execute(Player player, string[] args)
        {
            if (player.tradeTarget == null)
            {
                if (player.SwapBackpack(Convert.ToInt32(args[0])))
                {
                    player.SendInfo("Switched to backpack #" + args[0]);
                }
                else
                {
                    player.SendInfo("Backpack #" + args[0] + " does not exist!");
                }
            }
            else
            {
                player.SendError("Cannot switch backpack while trading!");
            }
        }
    }

}