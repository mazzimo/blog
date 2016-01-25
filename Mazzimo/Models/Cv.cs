using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Mazzimo.Models
{

    public class Cv
    {
        public string Intro {get;set;}
	    public string WorkingExperienceDesc {get;set;}
	    public string EducationDesc {get;set;}
	    public string SkillsDesc {get;set;}
        public string SideProjectsDesc { get; set; }
	    public List<WorkingExperience> WorkingExperience { get;set;}
        public List<Education> Education { get;set;}
        public List<Skill> Skills { get;set;}
        public List<SideProject> SideProjects { get; set; }
    }

    public class SideProject
    {
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public string PeriodDescription { get; set; }
        public string ProjectType { get; set; }
        public List<Skill> Skills { get; set; } 
    }

    public class WorkingExperience {
        public string CompanyName { get;set;}
        public string CompanyLogo { get;set;}
        public string CompanyWebsite { get; set; }
        public string RoleName { get;set;}
        public DateTime? DateStart { get;set;}
        public DateTime? DateEnd { get;set;}
        public string Description { get;set;}
        public List<Skill> Skills { get;set;}    
    }

    public class Skill {
        public string Id { get;set;}
        public string Desc { get;set;}
        public int? Percentage { get;set;}
    }

    public class Education { 
        public string Name {get;set;}
		public string Logo {get;set;}
        public string Website {get;set;}
        public int FromYear {get;set;}
        public int ToYear {get;set;}
        public string Degree {get;set;}
        public string Description {get;set;}    
    }
}