﻿namespace TeamBuilder.App.Core.Commands
{
    using System;
    using System.Linq;
    using Utilities;
    using Data;
    using Models;
    using TeamBuilder.App.Interfaces;

    public class InviteToTeamCommand : IExecutable
    {
        //•	InviteToTeam <teamName> <username>
        //        Sends an invite to the specified user to join given team.
        //If the user is actually the creator of the team – add him/her directly!
        public string Execute(string[] args)
        {
            Validator.CheckLength(2, args);
            AuthenticationManager.Authorize();
            string teamName = args[0];
            string username = args[1];

            if (!CommandHelper.IsTeamExisting(teamName) || !CommandHelper.IsUserExisting(username))
            {
                throw new ArgumentException(Constants.ErrorMessages.TeamOrUserNotExist);
            }
            if (CommandHelper.IsInvitePending(teamName, username))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.InviteIsAlreadySent);
            }

            if (!CommandHelper.IsCreatorOrPartOfTeam(teamName))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }
            this.SendInvitation(teamName, username);
            return $"Team {teamName} invited {username}!";
        }

        private void SendInvitation(string teamName, string username)
        {
            using (TeamBuilderContext ctx = new TeamBuilderContext())
            {
                Team team = ctx.Teams.FirstOrDefault(t => t.Name == teamName);
                User user = ctx.Users.FirstOrDefault(u => u.Username == username);
                Invitation inv = new Invitation
                {
                    TeamId = team.Id,
                    InvitedUserId = user.Id
                };
                if (team.CreatorId == user.Id)
                {
                    inv.IsActive = false;
                    team.Members.Add(user);
                }
                ctx.Invitations.Add(inv);
                ctx.SaveChanges();
            }
        }
    }
}
