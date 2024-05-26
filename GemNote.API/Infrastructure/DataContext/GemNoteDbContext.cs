﻿using GemNote.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GemNote.API.Infrastructure.DataContext;

public class GemNoteDbContext(DbContextOptions<GemNoteDbContext> options) : IdentityDbContext<AppUser>(options)
{
	public DbSet<AppUser> AppUsers { get; set; }
	public DbSet<Category> Categories { get; set; }
	public DbSet<Notebook> Notebooks { get; set; }
	public DbSet<Section> Sections { get; set; }
	public DbSet<Unit> Units { get; set; }
	public DbSet<Flashcard> Flashcards { get; set; }
	public DbSet<CardReviewSession> CardReviewSessions { get; set; }
	public DbSet<RefreshToken> RefreshTokens { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

	}
}