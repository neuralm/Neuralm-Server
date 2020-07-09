-- query to check which nodes should NOT be in the database
SELECT [Id]
      ,[Layer]
      ,[NodeIdentifier]
      ,[Discriminator] FROM [TrainingRoomDbContext].[dbo].[Node]
		LEFT JOIN [OrganismInputNode] 
		ON [Node].id = [OrganismInputNode].[InputNodeId]
		WHERE [OrganismInputNode].[InputNodeId] IS NULL
		AND [Node].[Discriminator] = 'InputNode'
UNION ALL
SELECT [Id]
      ,[Layer]
      ,[NodeIdentifier]
      ,[Discriminator] FROM [TrainingRoomDbContext].[dbo].[Node]
		LEFT JOIN [OrganismOutputNode] 
		ON [Node].id = [OrganismOutputNode].[OutputNodeId]
		WHERE [OrganismOutputNode].[OutputNodeId] IS NULL
		AND [Node].[Discriminator] = 'OutputNode'

-- query to check which nodes should be in the database
SELECT
        Node.[Id],
        Node.[Layer],
        Node.[NodeIdentifier],
        Node.[Discriminator] 
    FROM
        [TrainingRoomDbContext].[dbo].[Node] 
    INNER JOIN
        [OrganismInputNode] 
            ON [Node].id = [OrganismInputNode].[InputNodeId] 
    WHERE
        [OrganismInputNode].[InputNodeId] IS NOT NULL 
        AND [Node].[Discriminator] = 'InputNode' 
    UNION ALL
    SELECT
        Node.[Id],
        Node.[Layer],
        Node.[NodeIdentifier],
        Node.[Discriminator] 
    FROM
        [TrainingRoomDbContext].[dbo].[Node] 
    INNER JOIN
        [OrganismOutputNode] 
            ON [Node].id = [OrganismOutputNode].[OutputNodeId] 
    WHERE
        [OrganismOutputNode].[OutputNodeId] IS NOT NULL 
        AND [Node].[Discriminator] = 'OutputNode'