namespace Domain.Skills;

public class Skill
{
    public SkillId Id { get;}
    public string Title { get; private set; }
    
    private Skill(SkillId id, string title)
    {
        Id = id;
        Title = title;
    }
    
    public static Skill New(SkillId id, string title) 
        => new(id, title);
    
    public void UpdateDetails(string title) 
        => Title = title;
}