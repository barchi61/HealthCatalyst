using PeopleSearch.Data.EDM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace PeopleSearch.PeopleModule.Services
{
    public static class DataService
    {
        public static PeopleContext Context;

        public static EntityState GetEntityState(People people)
        {
            try
            {
                if (people != null)
                    return Context.Entry(people).State;
                else
                    return EntityState.Detached;
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        public static People AddPeople()
        {
            Context = new PeopleContext();
            Context.Configuration.AutoDetectChangesEnabled = true;

            try
            {
                return Context.People.Add(new People());
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        public static People GetPeopleById(long peopleId)
        {
            Context = new PeopleContext();
            Context.Configuration.AutoDetectChangesEnabled = true;

            try
            {
                var people = Context.People.Where(i => i.PeopleId == peopleId).FirstOrDefault();
                return people;

            }
            catch (Exception)
            {

                throw new NotImplementedException();
            }
        }

        public static People UndoChanges(long peopleId)
        {
            Context = new PeopleContext();
            Context.Configuration.AutoDetectChangesEnabled = true;

            try
            {
                var people = Context.People.Where(i => i.PeopleId == peopleId).FirstOrDefault();
                return people;

            }
            catch (Exception)
            {

                throw new NotImplementedException();
            }
        }

        public static ObservableCollection<People> GetData(string searchContext)
        {
            using (var context = new PeopleContext())
            {
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

                    ObservableCollection<People> peopleCollection = new ObservableCollection<People>(list);
                    return peopleCollection;
                }
                catch (Exception)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public static void Delete(long peopleId)
        {
            using (var context = new PeopleContext())
            {
                try
                {
                    var exisitingPeople = context.People.Where(i => i.PeopleId == peopleId).FirstOrDefault();
                    if (exisitingPeople != null)
                    {
                        context.People.Remove(exisitingPeople);
                        context.SaveChanges();
                    }

                }
                catch (Exception)
                {

                    throw new NotImplementedException();
                }
            }
        }

        public static void SaveChanges(People people)
        {
            using (var context = new PeopleContext())
            {
                try
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
                catch (Exception)
                {

                    throw new NotImplementedException();
                }
            }
        }
    }
}
