using Domain.Entity;
using Domain.Exception;
using Interface.Repository;
using Interface.Service;

namespace Businesslogic.Service;

/// <inheritdoc />
public class HumanReadableIdentifierService : IHumanReadableIdentifierService
{
    private const int WordCount = 2;
    private const int MaxAllowedGeneratorAttempts = 20;

    private static readonly List<string> Words = new()
    {
        "time", "year", "people", "way", "day", "man", "thing",
        "woman", "life", "child", "world", "school", "state",
        "family", "student", "group", "country", "problem",
        "hand", "part", "place", "case", "fact",
        "eye", "friend", "month", "truth", "marketing", "company",
        "story", "kid", "car", "book", "job", "word", "business",
        "issue", "side", "kind", "head", "house", "service",
        "father", "power", "hour", "game", "line", "end", "member",
        "law", "city", "community", "name", "president", "team",
        "minute", "idea", "body", "information", "back", "parent",
        "face", "others", "level", "office", "door", "health", "person",
        "art", "war", "history", "party", "result", "change", "morning",
        "reason", "research", "girl", "guy", "moment", "air", "teacher",
        "force", "education", "foot", "boy", "age", "policy", "music",
        "food", "reading", "data", "sea", "role", "space", "ground",
        "event", "series", "dealer", "image", "computer", "glass", "table",
        "piece", "land", "system", "university", "theory", "god",
        "machine", "season", "view", "relation", "investment",
        "film", "oil", "situation", "speech", "figure", "street", "wall",
        "tree", "source", "farmer", "river", "money", "class", "road",
        "map", "science", "color", "strategy", "police", "mind",
        "army", "camera", "freedom", "paper", "environment", "instance",
        "writing", "article", "department", "difference", "goal", "news", "audience",
        "fishing", "growth", "income", "marriage", "user", "combination",
        "failure", "meaning", "medicine", "philosophy", "communication",
        "night", "chemistry", "disease", "disk", "energy", "nation",
        "soup", "success", "addition", "apartment", "math",
        "painting", "politics", "attention", "decision", "property",
        "shopping", "wood", "competition", "distribution", "entertainment",
        "population", "unit", "category", "cigarette", "context",
        "introduction", "opportunity", "performance", "driver",
        "flight", "length", "magazine", "newspaper", "relationship",
        "teaching", "cell", "debate", "finding", "lake",
        "message", "phone", "scene", "appearance", "association",
        "concept", "customer", "death", "discussion", "housing",
        "inflation", "insurance", "mood", "advice", "blood", "effort",
        "expression", "importance", "opinion", "payment", "reality",
        "responsibility", "situation", "skill", "statement", "wealth",
        "application", "county", "depth", "estate", "foundation",
        "grandmother", "heart", "perspective", "photo", "recipe",
        "studio", "topic", "collection", "depression", "imagination",
        "passion", "percentage", "resource", "setting", "ad", "agency",
        "college", "connection", "criticism", "debt", "description",
        "memory", "patience", "secretary", "solution", "administration",
        "aspect", "attitude", "director", "personality", "psychology",
        "recommendation", "response", "selection", "storage", "version",
        "alcohol", "argument", "complaint", "contract", "emphasis",
        "highway", "loss", "membership", "possession", "preparation",
        "steak", "union", "agreement", "cancer", "currency", "employment",
        "engineering", "entry", "interaction", "limit", "mixture",
        "preference", "region", "republic", "seat", "tradition",
        "virus", "actor", "classroom", "delivery", "device", "difficulty",
        "drama", "election", "engine", "football", "guidance", "hotel",
        "owner", "priority", "protection", "suggestion", "tension",
        "variation", "anxiety", "atmosphere", "awareness", "bread",
        "climate", "comparison", "confusion", "construction", "elevator",
        "emotion", "employee", "employer", "guest", "height", "leadership",
        "mall", "manager", "operation", "recording", "sample", "transportation",
        "charity", "cousin", "disaster", "editor", "efficiency",
        "excitement", "extent", "feedback", "guitar", "homework", "leader",
        "mom", "outcome", "permission", "presentation", "promotion",
        "reflection", "refrigerator", "resolution", "revenue", "session",
        "singer", "tennis", "basket", "bonus", "cabinet", "childhood",
        "church", "clothes", "coffee", "dinner", "drawing", "hair",
        "hearing", "initiative", "judgment", "lab", "measurement",
        "mode", "mud", "orange", "poetry", "possibility", "procedure",
        "queen", "ratio",
    };

    private readonly ILobbyRepository lobbyRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="HumanReadableIdentifierService"/> class.
    /// </summary>
    /// <param name="lobbyRepository">Lobby repository.</param>
    public HumanReadableIdentifierService(
        ILobbyRepository lobbyRepository)
    {
        this.lobbyRepository = lobbyRepository;
    }

    /// <inheritdoc />
    public async Task<string> GenerateUniqueIdentifier()
    {
        var random = new Random();
        string nonUniqueIdentifier;
        Lobby? exsistingLobby = default;
        var counter = 0;

        do
        {
            nonUniqueIdentifier = this.RandomHumanReadableIdentifier(random);
            try
            {
                exsistingLobby = await this.lobbyRepository.GetLobby(nonUniqueIdentifier);
            }
            catch (RepositoryException)
            {
                counter++;
                if (counter > MaxAllowedGeneratorAttempts)
                {
                    throw new ServiceException(
                        $"Did not find a unique human-readable-identifier after {MaxAllowedGeneratorAttempts} attempts");
                }
            }
        }
        while (exsistingLobby is not null);

        return nonUniqueIdentifier; // actually unique at this point.
    }

    private string RandomHumanReadableIdentifier(Random? ran = null)
    {
        var random = ran ?? new Random();
        return string.Join("-", Enumerable.Range(0, WordCount)
            .Select(_ => Words[random.Next(Words.Count)]));
    }
}