﻿using GigHubMVC.Models;
using GigHubMVC.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GigHubMVC.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IGigRepository Gigs { get; private set; }
        public IAttendanceRepository Attendances { get; private set; }
        public IFollowingRepository Followings { get; private set; }
        public IGenreRepository Genres { get; private set; }
        public IApplicationUserRepository Users { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Gigs = new GigRepository(_context);
            Attendances = new AttendanceRepository(_context);
            Followings = new FollowingRepository(_context);
            Genres = new GenreRepository(_context);
            Users = new ApplicationUserRepository(_context);
        }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}