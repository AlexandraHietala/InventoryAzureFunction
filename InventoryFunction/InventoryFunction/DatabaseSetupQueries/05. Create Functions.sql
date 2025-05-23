-----------------------------------------------------------

-- Notes:


-----------------------------------------------------------

USE [SEInventoryDB]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-----------------------------------------------------------

CREATE FUNCTION [dbo].[fnSplit_OnPipe]
(
   @list varchar(max)
)
RETURNS TABLE
AS

   RETURN 
   (
	   SELECT [SplitData] = CONVERT(INT, [SplitData])
       FROM
       (
           SELECT [SplitData] = [x].[i].value('(./text())[1]', 'INT')
           FROM
           (
               SELECT [compiled] = CONVERT(XML, '<i>' + REPLACE(@list, '|', '</i><i>') + '</i>').query('.')
           ) AS [xml]
           CROSS APPLY
           [compiled].nodes('i') AS [x]([i])
       ) AS [split]
       WHERE [SplitData] IS NOT NULL
   );

   GO

 -----------------------------------------------------------

CREATE FUNCTION [dbo].[fnSplit_KeyValuePairs]
(
   @list varchar(max)
)
RETURNS TABLE
AS

   RETURN 
   (
       SELECT [Key], [Value]
       FROM
       (
           SELECT [Key] = [y].[k].value('.', 'VARCHAR(max)'), [Value] = [z].[v].value('.', 'VARCHAR(max)')
           FROM
           (
               SELECT [compiled] = CONVERT(XML, REPLACE('<items><i><k>' + REPLACE(@list, '||', '</v></i><i><k>') + '</v></i></items>', '::', '</k><v>')).query('.')
           ) AS [xml]
           CROSS APPLY
            [compiled].nodes('items/i') AS [items]([i])
		   CROSS APPLY
           [items].[i].nodes('k') AS [y]([k])
		   CROSS APPLY
           [items].[i].nodes('v') AS [z]([v])
       ) AS [split]
       WHERE [Key] IS NOT NULL
   );

GO

-----------------------------------------------------------
