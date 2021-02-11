using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoyaleTrackerAPI.Repos
{
    interface ITeamsRepo
    {
        //Create+Read returns a team with a set Id, Creates a team if there isn't one
        Team GetSetTeamId(List<TeamMember> team);
        Team GetSetTeamId(Player player);
        Team GetSetTeamId(Team team);

        Team GetTeamById(int id);
        List<Team> GetAllTeams();

        //Update
        void UpdateTeam(Team team);

        //Delete
        void DeleteTeam(int teamID);

    }
}
