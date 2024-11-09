using FluentValidation;
using FluentValidation.AspNetCore;
using Project.Configurations;
using Project.Configurations.Conventions;
using Project.Core.Entities.Options.DTOs;
using Project.Core.Entities.Options.Models;
using Project.Core.Entities.Polls.DTOs;
using Project.Core.Entities.Polls.Models;
using Project.Core.Entities.Products.DTOs;
using Project.Core.Entities.Products.Models;
using Project.Core.Entities.Products.Validation.DTOs.Products;
using Project.Core.Mapping.Implementations.Products;
using Project.Core.Mapping.Interface;
using Project.Core.Utils.ErrorMsg.Base;
using Project.Core.Utils.ErrorMsg.Interface;
using Project.DataAccess.Interface;
using Project.DataAccess.Repositories.InMemory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddCorsPolicy();
builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddValidationServices();

builder.Services.AddControllers(options =>
{
  options.Conventions.Add(new LowercaseControllerConvention());
})
.AddJsonOptions(options =>
{
  options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

builder.Services.AddAuthorization();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddValidatorsFromAssemblyContaining<PollOptionDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<PollDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterProductDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<VoteDTOValidator>();

builder.Services.AddScoped<IRequiredValidationMsg, RequiredValService>();
builder.Services.AddScoped<IFormatValidationMsg, FormatValService>();
builder.Services.AddScoped<ITypeValuesValidationMsg, TypeValuesValService>();
builder.Services.AddScoped<IContentValidationMsg, ContentValService>();
builder.Services.AddScoped<IRangeValidationMsg, RangeValService>();

builder.Services.AddTransient<PollOptionDTOValidator>();
builder.Services.AddTransient<PollDTOValidator>();
builder.Services.AddTransient<RegisterProductDTOValidator>();
builder.Services.AddTransient<VoteDTOValidator>();

builder.Services.AddScoped<IMapper<Product, RegisterProductDTO, UpdateProductDTO>, ProductMapper>();
builder.Services.AddScoped<IRepository<Product, RegisterProductDTO, UpdateProductDTO>, ProductRepository>();

builder.Services.AddScoped<IMapper<Poll, RegisterPollDTO, UpdatePollDTO>, PollMapper>();
builder.Services.AddScoped<IRepository<Poll, RegisterPollDTO, UpdatePollDTO>, PollRepository>();

builder.Services.AddScoped<IMapper<PollOption, RegisterPollOptionsDTO, UpdatePollOptionsDTO>, PollOptionMapper>();
builder.Services.AddScoped<IRepository<PollOption, RegisterPollOptionsDTO, UpdatePollOptionsDTO>, PollOptionRepository>();

builder.Services.AddScoped<IMapper<PollOption, RegisterPollOptionsDTO, UpdatePollOptionsDTO>, PollOptionMapper>();
builder.Services.AddScoped<IRepository<PollOption, RegisterPollOptionsDTO, UpdatePollOptionsDTO>, PollOptionRepository>();

builder.Services.AddScoped<IMapper<Vote, RegisterVoteDTO, UpdateVoteDTO>, VoteMapper>();
builder.Services.AddScoped<IRepository<Vote, RegisterVoteDTO, UpdateVoteDTO>, VoteRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI(c =>
  {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Project V1");
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "Api Project V2");
  });
}

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAllOrigins");
app.MapControllers();
app.Run();