﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Der Code wurde von einer Vorlage generiert.
//
//     Manuelle Änderungen an dieser Datei führen möglicherweise zu unerwartetem Verhalten der Anwendung.
//     Manuelle Änderungen an dieser Datei werden überschrieben, wenn der Code neu generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Autopilot
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class AutopilotEntities : DbContext
    {
        public AutopilotEntities()
            : base("name=AutopilotEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ablehnungsgrund> ablehnungsgrund { get; set; }
        public virtual DbSet<anrede> anrede { get; set; }
        public virtual DbSet<auftrag> auftrag { get; set; }
        public virtual DbSet<auftragsart> auftragsart { get; set; }
        public virtual DbSet<buchung> buchung { get; set; }
        public virtual DbSet<firmenstammdaten> firmenstammdaten { get; set; }
        public virtual DbSet<flughafen> flughafen { get; set; }
        public virtual DbSet<flugzeug> flugzeug { get; set; }
        public virtual DbSet<flugzeugtyp> flugzeugtyp { get; set; }
        public virtual DbSet<hersteller> hersteller { get; set; }
        public virtual DbSet<kunde> kunde { get; set; }
        public virtual DbSet<kundengruppe> kundengruppe { get; set; }
        public virtual DbSet<mahnstufe> mahnstufe { get; set; }
        public virtual DbSet<personal> personal { get; set; }
        public virtual DbSet<phonetisch> phonetisch { get; set; }
        public virtual DbSet<position> position { get; set; }
        public virtual DbSet<positionsart> positionsart { get; set; }
        public virtual DbSet<statusgruppe> statusgruppe { get; set; }
        public virtual DbSet<termin> termin { get; set; }
        public virtual DbSet<terminart> terminart { get; set; }
        public virtual DbSet<titel> titel { get; set; }
        public virtual DbSet<triebwerksart> triebwerksart { get; set; }
        public virtual DbSet<lizenzen> lizenzen { get; set; }
        public virtual DbSet<status> status { get; set; }
        public virtual DbSet<termin_flugzeug> termin_flugzeug { get; set; }
        public virtual DbSet<termin_personal> termin_personal { get; set; }
        public virtual DbSet<wunschcrew> wunschcrew { get; set; }
        public virtual DbSet<wunschflugzeug> wunschflugzeug { get; set; }
        public virtual DbSet<zwischenlandung> zwischenlandung { get; set; }
        public virtual DbSet<flugzeugliste> flugzeugliste { get; set; }
        public virtual DbSet<personalliste> personalliste { get; set; }
        public virtual DbSet<termin_auftrag> termin_auftrag { get; set; }
        public virtual DbSet<Kundenliste> Kundenliste { get; set; }
    }
}
