using EcommerceApi.Data;
using EcommerceApi.Repositories.Interfaces;
using EcommerceApi.Repositories;
using EcommerceApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configurar Kestrel para escuchar en el puerto asignado por Render o el 8080 por defecto
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.ConfigureKestrel(options =>
{
	options.ListenAnyIP(int.Parse(port));
});

// Configurar el DbContext con la cadena de conexión
builder.Services.AddDbContext<tienda_mayoristaContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Servicios y repositorios
builder.Services.AddScoped<ProductoService>();
builder.Services.AddScoped<ItemCarritoService>();
builder.Services.AddScoped<CarritoService>();
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<ItemOrdeneService>();
builder.Services.AddScoped<OrdeneService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IItemCarritoRepository, ItemCarritoRepository>();
builder.Services.AddScoped<ICarritoRepository, CarritoRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IItemOrdeneRepository, ItemOrdeneRepository>();
builder.Services.AddScoped<IOrdeneRepository, OrdeneRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

// Configuración de JWT
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(x =>
{
	x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
	x.RequireHttpsMetadata = false;
	x.SaveToken = true;
	x.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(key),
		ValidateIssuer = false,
		ValidateAudience = false
	};
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ecommerce API", Version = "v1" });
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Por favor ingresa un token válido",
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		Scheme = "bearer"
	});
	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			new string[] {}
		}
	});
});

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", policy =>
	{
		policy.AllowAnyOrigin()
			  .AllowAnyHeader()
			  .AllowAnyMethod();
	});
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// **Comenta esta línea mientras haces pruebas en Render (si da problemas HTTPS)**
// app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
