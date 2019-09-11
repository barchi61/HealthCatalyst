using PeopleSearch.Data.EDM;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;

namespace PeopleSearch.PeopleModule.Services
{
    public static class DataService
    {
        public static PeopleContext Context;

        public static EntityState GetEntityState(People people)
        {
            return Context.Entry(people).State;
        }

        public static People GetPeopleById(long peopleId)
        {
            Context = new PeopleContext();
            Context.Configuration.AutoDetectChangesEnabled = true;

            var people = Context.People.Where(i => i.PeopleId == peopleId).FirstOrDefault();
            return people;
        }

        public static People UndoChanges(long peopleId)
        {
            Context = new PeopleContext();
            Context.Configuration.AutoDetectChangesEnabled = true;

            var people = Context.People.Where(i => i.PeopleId == peopleId).FirstOrDefault();
            return people;

        }

        public static ObservableCollection<People> GetData(string searchContext)
        {
            var context = new PeopleContext();
            IQueryable<People> query;
            List<People> list = new List<People>();

            searchContext = (searchContext == null) ? string.Empty : searchContext;

            try
            {
                switch (searchContext)
                {
                    case string s when s == string.Empty:
                        query = from a in context.People
                                select a;

                        list = query.ToList<People>();
                        break;
                    case string s when s != string.Empty && !s.Contains(' '):
                        query = from a in context.People
                                where a.FirstName.ToUpper().Contains(searchContext.ToUpper())
                                    || a.LastName.ToUpper().Contains(searchContext.ToUpper())
                                select a;

                        list = query.ToList<People>();
                        break;
                    case string s when s.Contains(' '):
                        query = from a in context.People
                                select a;

                        var results = query.ToList();

                        // Process locally since SQL can not interpret Split statements
                        var searchStrings = searchContext.ToUpper().Split(' ').ToList();
                        list = results
                            .Where(a => (a.FirstName + " " + a.LastName).Split(' ')
                            .Any(b => searchStrings.Any(c => b.Contains(c)))).ToList();
                        break;
                }
            }
            catch (Exception)
            {
            }

            ObservableCollection<People> peopleCollection = new ObservableCollection<People>(list);
            return peopleCollection;
        }

        public static void Delete(long peopleId)
        {
            using (var context = new PeopleContext())
            {
                var exisitingPeople = context.People.Where(i => i.PeopleId == peopleId).FirstOrDefault();
                if (exisitingPeople != null)
                {
                    context.People.Remove(exisitingPeople);
                    context.SaveChanges();
                }
            }
        }

        public static void SaveChanges(People people)
        {
            using (var context = new PeopleContext())
            {
                var exisitingPeople = context.People.Where(i => i.PeopleId == people.PeopleId).FirstOrDefault();
                if (exisitingPeople == null)
                {
                    context.People.Add(people);
                }
                else
                {
                    context.Entry(exisitingPeople).CurrentValues.SetValues(people);
                }

                context.SaveChanges();
            }
        }

    }
}
