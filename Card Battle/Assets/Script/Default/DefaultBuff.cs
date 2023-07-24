public enum BuffType
{
    BUFF,
    DEBUFF
}
public class DefaultBuff
{
    int id;
    public int Id { get => id; }

    BuffType type;
    public BuffType Type { get => type; }

    string name;
    public string Name { get => name; }

    int turns;
    public int Turns { get => turns; set => turns = value; }

    int currentTurn;
    public int CurrentTurn { get => currentTurn; set => currentTurn = value; }

    public delegate void Use(Character character); //버프 적용 함수
    public Use use;
    public DefaultBuff(int id, BuffType type, string name, int turns)
    {
        this.id = id;
        this.type = type;
        this.name = name;
        this.turns = turns;
        currentTurn = turns;
    }
}
