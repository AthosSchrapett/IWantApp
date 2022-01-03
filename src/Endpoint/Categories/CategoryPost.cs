using IWantApp.Domain.Products;
using IWantApp.Infra.Data;

namespace IWantApp.Endpoint.Categories;

public class CategoryPost
{
    public static string Template => "/categories";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(CategoryRequest categoryRequest, ApplicationDbContext context)
    {
        var category = new Category(categoryRequest.Name, "Test", "Teste");

        if (!category.IsValid)
            return Results.ValidationProblem(category.Notifications.ConvertToProblemDatails());            

        context.Categories.Add(category);
        context.SaveChanges();

        return Results.Created($"/categories/{category.Id}", category.Id);
    }

}
