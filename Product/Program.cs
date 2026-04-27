using Microsoft.EntityFrameworkCore;
using Product.Data;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register EF Core with SQL Server (or SQLite for local dev)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
// For SQLite (no SQL Server needed): options.UseSqlite("Data Source=products.db")
);

var app = builder.Build();

// Auto-apply migrations on startup (optional but handy for dev)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated(); // Creates DB if it doesn't exist
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // reads the token
app.UseAuthorization();  // checks if allowed

app.MapControllers();
app.Run();