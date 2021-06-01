IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210601100648_InitialCreate')
BEGIN
    CREATE TABLE [UserVocabs] (
        [Id] int NOT NULL IDENTITY,
        [ChatId] int NOT NULL,
        CONSTRAINT [PK_UserVocabs] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210601100648_InitialCreate')
BEGIN
    CREATE TABLE [VocabItems] (
        [VocabItemId] int NOT NULL IDENTITY,
        [EnglishWord] nvarchar(max) NOT NULL,
        [Translation] nvarchar(max) NULL,
        [Explanation] nvarchar(max) NULL,
        [UserVocabId] int NOT NULL,
        CONSTRAINT [PK_VocabItems] PRIMARY KEY ([VocabItemId]),
        CONSTRAINT [FK_VocabItems_UserVocabs_UserVocabId] FOREIGN KEY ([UserVocabId]) REFERENCES [UserVocabs] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210601100648_InitialCreate')
BEGIN
    CREATE INDEX [IX_VocabItems_UserVocabId] ON [VocabItems] ([UserVocabId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210601100648_InitialCreate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20210601100648_InitialCreate', N'5.0.5');
END;
GO

COMMIT;
GO

