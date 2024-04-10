using DomainModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
namespace WebMvc.Database;

public class BusDb : DbContext
{
    public BusDb(DbContextOptions<BusDb> options)
    : base(options)
    { }
    public DbSet<Bus> Bus { get; set; }
    public DbSet<Driver> Driver { get; set; }
    public DbSet<Entry> Entry { get; set; }
    public DbSet<Loop> Loop { get; set; }
    public DbSet<Route> Route { get; set; }
    public DbSet<Stop> Stop { get; set; }
    public DbSet<User> User { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    => options.UseSqlite($"Data Source=BusDb.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Route>()
            .HasOne(r => r.Stop)
            .WithMany()
            .HasForeignKey(r => r.StopId)
            .OnDelete(DeleteBehavior.Restrict); // Or .Cascade as needed

        modelBuilder.Entity<Route>()
            .HasOne(r => r.Loop)
            .WithMany()
            .HasForeignKey(r => r.LoopId)
            .OnDelete(DeleteBehavior.Restrict); // Or .Cascade as needed

        modelBuilder.Entity<Entry>()
        .HasOne(r => r.Loop)
        .WithMany()
        .HasForeignKey(r => r.LoopId)
        .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Entry>()
      .HasOne(r => r.Stop)
      .WithMany()
      .HasForeignKey(r => r.StopId)
      .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Entry>()
      .HasOne(r => r.Driver)
      .WithMany()
      .HasForeignKey(r => r.DriverId)
      .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Entry>()
      .HasOne(r => r.Bus)
      .WithMany()
      .HasForeignKey(r => r.BusId)
      .OnDelete(DeleteBehavior.Restrict);
    }

}
public class Bus
{
    public int Id { get; set; }
    public int BusNumber { get; set; }
}

public class Driver
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
public class Entry
{
    public int Id { get; set; }
    public DateTime TimeStamp { get; set; }
    public int Boarded { get; set; }
    public int LeftBehind { get; set; }
    public int StopId { get; set; }
    public Stop Stop { get; set; }
    public int LoopId { get; set; }
    public Loop Loop { get; set; }
    public int DriverId { get; set; }
    public Driver Driver { get; set; }
    public int BusId { get; set; }
    public Bus Bus { get; set; }
}
public class Loop
{
    public int Id { get; set; }
    public string Name { get; set; }
}
public class Route
{
    public int Id { get; set; }
    public int Order { get; set; }
    public int StopId { get; set; }
    public Stop Stop { get; set; }
    public int LoopId { get; set; }
    public Loop Loop { get; set; }
}
public class Stop
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}