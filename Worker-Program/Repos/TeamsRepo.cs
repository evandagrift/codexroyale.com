using ClashFeeder.Models;
using ClashFeeder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClashFeeder.Repos
{
    public class TeamsRepo 
    {
        //DB Access
        private TRContext context;
        //constructor assigns the argument DB context
        public TeamsRepo(TRContext c) { context = c; }


        //Team is something I constructed outside of the ClashRoyale Api
        //this will get or set a teams Id off a list of teammembers (How team data is intaken from the official API)
        public Team GetSetTeamId(List<TeamMember> teamMembers)
        {

            if (teamMembers != null)
            {
                //defaults to 1 PlayerSnapshot
                bool twoVtwo = false;

                //if there are two PlayerSnapshots in the team members list, sets to 2v2
                if (teamMembers.Count == 2) twoVtwo = true;

                int teamId = 0;
                Team teamToReturn = new Team();

                //if there are teams in the DB
                if (context.Teams.Count() > 0)
                {

                    //searches db for all teams and focuses that search based on wether on not it's 2v2
                    var teams = context.Teams.Where(t => t.TwoVTwo == twoVtwo);

                    //if it's no 2v2
                    if (!twoVtwo)
                    {
                        teamToReturn = teams.Where(t => t.Tag == teamMembers[0].Tag).FirstOrDefault();
                    }
                    else
                    {
                        teamToReturn = teams.Where(t => ((t.Tag == teamMembers[0].Tag && t.Tag2 == teamMembers[1].Tag) || (t.Tag == teamMembers[1].Tag && t.Tag2 == teamMembers[0].Tag))).FirstOrDefault();

                    }
                    if (teamToReturn != null) { teamId = teamToReturn.TeamId; }

                }

                //if a team hasn't been found with your specifications it creates a new one
                if (teamId == 0)
                {
                    Team newTeam = new Team();
                    newTeam = new Team();

                    newTeam.Tag = teamMembers[0].Tag;
                    newTeam.Name = teamMembers[0].Name;
                    newTeam.TeamName = teamMembers[0].Name;

                    if (twoVtwo)
                    {
                        newTeam.TwoVTwo = true;
                        newTeam.Tag2 = teamMembers[1].Tag;
                        newTeam.Name2 = teamMembers[1].Name;
                        newTeam.TeamName += " " + teamMembers[1].Name;
                    }

                    context.Teams.Add(newTeam);
                    context.SaveChanges();
                    teamToReturn = newTeam;
                }
                return teamToReturn;
            }
            else return null;

        }

        // any unique team of PlayerSnapshots, a, b, a+b... is saved and given a team Id by the database
        public Team GetSetTeamId(PlayerSnapshot playerSnapshot)
        {
            List<TeamMember> t = new List<TeamMember>();
            TeamMember p = new TeamMember();
            p.Tag = playerSnapshot.Tag;
            p.Name = playerSnapshot.Name;
            t.Add(p);
            return GetSetTeamId(t);
        }
        // any unique team of PlayerSnapshots, a, b, a+b... is saved and given a team Id by the database
        public Team GetSetTeamId(Team team)
        {
            List<TeamMember> t = new List<TeamMember>();

            t.Add(new TeamMember() { Tag = team.Tag, Name = team.Name });
            t.Add(new TeamMember() { Tag = team.Tag2, Name = team.Name2 });

            return GetSetTeamId(t);
        }


        //returns list of all teams from DB
        public List<Team> GetAllTeams() { return context.Teams.ToList(); }

        //returns Team with given ID from DB
        public Team GetTeamById(int teamID) { return context.Teams.Find(teamID); }



        //updates team at given ID with properties from given argument
        public void UpdateTeam(Team team)
        {
            //fetches team from database
            Team teamToUpdate = GetTeamById(team.TeamId);

            //changes fetched instance from the DB
            if (teamToUpdate != null)
            {
                teamToUpdate.TeamName = team.TeamName;
                teamToUpdate.TwoVTwo = team.TwoVTwo;
                teamToUpdate.Name = team.Name;
                teamToUpdate.Tag = team.Tag;
                teamToUpdate.Name2 = team.Name2;
                teamToUpdate.Tag2 = team.Tag2;
                context.SaveChanges();
            }

        }

        //deletes the team at given ID from the DB
        public void DeleteTeam(int teamId)
        {
            Team team = GetTeamById(teamId);
            if(team!= null)
            {
                context.Teams.Remove(team);
                context.SaveChanges();  
            }
        }

    }
}
