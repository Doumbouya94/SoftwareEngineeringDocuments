using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventGoApp.Models;
using EventGoApp.Services;
using NSubstitute;

namespace EventGoApp.Tests;

/// <summary>
/// Tests unitaires pour le Pattern Commande des favoris.
/// Couvre AddFavoriteCommand, RemoveFavoriteCommand, FavoriteCommandInvoker.
/// </summary>
public class FavoriteCommandTests
{
    // ─── AddFavoriteCommand ───────────────────────────────────────────────

    [Fact]
    public async Task AddFavoriteCommand_Execute_ShouldCall_AddAsync_WithCorrectEvent()
    {
        var repo = Substitute.For<IFavoriteRepository>();
        var ev = new Event { Id = Guid.NewGuid(), Title = "Test" };
        var cmd = new AddFavoriteCommand(ev, repo);

        await cmd.Execute();

        await repo.Received(1).AddAsync(ev);
    }

    [Fact]
    public async Task AddFavoriteCommand_Undo_ShouldCall_RemoveAsync_WithCorrectId()
    {
        var repo = Substitute.For<IFavoriteRepository>();
        var id = Guid.NewGuid();
        var ev = new Event { Id = id };
        var cmd = new AddFavoriteCommand(ev, repo);

        await cmd.Undo();

        await repo.Received(1).RemoveAsync(id);
    }

    // ─── RemoveFavoriteCommand ────────────────────────────────────────────

    [Fact]
    public async Task RemoveFavoriteCommand_Execute_ShouldCall_RemoveAsync_WithCorrectId()
    {
        var repo = Substitute.For<IFavoriteRepository>();
        var id = Guid.NewGuid();
        var ev = new Event { Id = id };
        var cmd = new RemoveFavoriteCommand(ev, repo);

        await cmd.Execute();

        await repo.Received(1).RemoveAsync(id);
    }

    [Fact]
    public async Task RemoveFavoriteCommand_Undo_ShouldCall_AddAsync_WithCorrectEvent()
    {
        var repo = Substitute.For<IFavoriteRepository>();
        var ev = new Event { Id = Guid.NewGuid(), Title = "Concert" };
        var cmd = new RemoveFavoriteCommand(ev, repo);

        await cmd.Undo();

        await repo.Received(1).AddAsync(ev);
    }

    // ─── FavoriteCommandInvoker ───────────────────────────────────────────

    [Fact]
    public async Task Invoker_ExecuteAsync_ShouldCall_Execute_AndPushToHistory()
    {
        var invoker = new FavoriteCommandInvoker();
        var cmd = Substitute.For<IFavoriteCommand>();

        await invoker.ExecuteAsync(cmd);

        await cmd.Received(1).Execute();
    }

    [Fact]
    public async Task Invoker_UndoLastAsync_ShouldCall_Undo_OnLastCommand()
    {
        var invoker = new FavoriteCommandInvoker();
        var cmd = Substitute.For<IFavoriteCommand>();
        await invoker.ExecuteAsync(cmd);

        await invoker.UndoLastAsync();

        await cmd.Received(1).Undo();
    }

    [Fact]
    public async Task Invoker_UndoLastAsync_ShouldRespect_LIFOOrder()
    {
        var invoker = new FavoriteCommandInvoker();
        var cmd1 = Substitute.For<IFavoriteCommand>();
        var cmd2 = Substitute.For<IFavoriteCommand>();
        await invoker.ExecuteAsync(cmd1);
        await invoker.ExecuteAsync(cmd2);

        await invoker.UndoLastAsync();

        await cmd2.Received(1).Undo();
        await cmd1.DidNotReceive().Undo();
    }

    [Fact]
    public async Task Invoker_UndoLastAsync_ShouldNotThrow_WhenHistoryIsEmpty()
    {
        var invoker = new FavoriteCommandInvoker();

        var ex = await Record.ExceptionAsync(() => invoker.UndoLastAsync());

        Assert.Null(ex);
    }

    [Fact]
    public void Invoker_CanUndo_ShouldBeFalse_WhenHistoryIsEmpty()
    {
        var invoker = new FavoriteCommandInvoker();

        Assert.False(invoker.CanUndo);
    }

    [Fact]
    public async Task Invoker_CanUndo_ShouldBeTrue_AfterExecute()
    {
        var invoker = new FavoriteCommandInvoker();
        var cmd = Substitute.For<IFavoriteCommand>();

        await invoker.ExecuteAsync(cmd);

        Assert.True(invoker.CanUndo);
    }
}