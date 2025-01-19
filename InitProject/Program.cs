var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.MapGet("/clothingInfo", () =>
{
    return @"Clothing (also known as clothes, garments, dress, apparel, or attire) is any item worn on the body. 
    Typically, clothing is made of fabrics or textiles, but over time it has included garments made from animal skin and other thin sheets of materials and natural products found in the environment, put together.
    The wearing of clothing is mostly restricted to human beings and is a feature of all human societies. 
    The amount and type of clothing worn depends on gender, body type, social factors, and geographic considerations.";
});

app.Run();
