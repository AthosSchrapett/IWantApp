using IWantApp.Domain.Products;
using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Mvc;

namespace IWantApp.Endpoint.Categories;

public class CategoryPut
{
    public static string Template => "/categories/{id:Guid}";
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action([FromRoute] Guid id, CategoryRequest categoryRequest, ApplicationDbContext context)
    {
        var category = context.Categories.Where(c => c.Id == id).FirstOrDefault();

        if (category == null)
            return Results.NotFound();

        category.EditInfo(categoryRequest.Name, categoryRequest.Active);

        if(!category.IsValid)
            return Results.ValidationProblem(category.Notifications.ConvertToProblemDatails());

        context.SaveChanges();

        return Results.Ok();
    }

}
