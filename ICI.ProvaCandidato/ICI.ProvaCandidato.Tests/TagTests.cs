using System;
using Xunit;
using ICiProvaCandidato.Dominio.Entidades;

namespace ICI.ProvaCandidato.Tests
{
    public class TagTests
    {
        // 1. Constructor Tests
        [Fact]
        public void Constructor_ValidNomeAndDescricao_SetsPropertiesCorrectly()
        {
            // Arrange
            var nome = "Tecnologia";
            var descricao = "Notícias sobre tecnologia";

            // Act
            var tag = new Tag(nome, descricao);

            // Assert
            Assert.Equal(nome, tag.Nome);
            Assert.Equal(descricao, tag.Descricao);
            Assert.True(tag.Ativa);
        }

        [Fact]
        public void Constructor_ValidNomeNullDescricao_SetsDescricaoToEmptyAndAtivaTrue()
        {
            // Arrange
            var nome = "Esportes";

            // Act
            var tag = new Tag(nome, null);

            // Assert
            Assert.Equal(nome, tag.Nome);
            Assert.Equal(string.Empty, tag.Descricao);
            Assert.True(tag.Ativa);
        }
        
        [Fact]
        public void Constructor_ValidNomeOmittedDescricao_SetsDescricaoToEmptyAndAtivaTrue()
        {
            // Arrange
            var nome = "Finanças";

            // Act
            var tag = new Tag(nome); // Descricao is omitted, should use default value ""

            // Assert
            Assert.Equal(nome, tag.Nome);
            Assert.Equal(string.Empty, tag.Descricao); // Default value for descricao is string.Empty
            Assert.True(tag.Ativa);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_InvalidNome_ThrowsArgumentException(string invalidNome)
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => new Tag(invalidNome, "Alguma Descrição"));
            Assert.Equal("nome", exception.ParamName);
        }

        [Fact]
        public void Constructor_NomeExceedsMaxLength_ThrowsArgumentException()
        {
            // Arrange
            var longNome = new string('a', 51);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => new Tag(longNome, "Alguma Descrição"));
            Assert.Equal("nome", exception.ParamName);
        }

        // 2. AtualizarNome Method Tests
        [Fact]
        public void AtualizarNome_ValidNewNome_UpdatesNomeProperty()
        {
            // Arrange
            var tag = new Tag("NomeAntigo", "Descrição");
            var novoNome = "NomeNovo";

            // Act
            tag.AtualizarNome(novoNome);

            // Assert
            Assert.Equal(novoNome, tag.Nome);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void AtualizarNome_InvalidNewNome_ThrowsArgumentExceptionAndNomeUnchanged(string invalidNome)
        {
            // Arrange
            var nomeOriginal = "NomeOriginal";
            var tag = new Tag(nomeOriginal, "Descrição");

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => tag.AtualizarNome(invalidNome));
            Assert.Equal("nome", exception.ParamName);
            Assert.Equal(nomeOriginal, tag.Nome); // Verify Nome remains unchanged
        }

        [Fact]
        public void AtualizarNome_NewNomeExceedsMaxLength_ThrowsArgumentExceptionAndNomeUnchanged()
        {
            // Arrange
            var nomeOriginal = "NomeOriginal";
            var tag = new Tag(nomeOriginal, "Descrição");
            var longNome = new string('b', 51);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => tag.AtualizarNome(longNome));
            Assert.Equal("nome", exception.ParamName);
            Assert.Equal(nomeOriginal, tag.Nome); // Verify Nome remains unchanged
        }

        // 3. AtualizarDescricao Method Tests
        [Fact]
        public void AtualizarDescricao_ValidNewDescricao_UpdatesDescricaoProperty()
        {
            // Arrange
            var tag = new Tag("NomeValido", "DescricaoAntiga");
            var novaDescricao = "DescricaoNova";

            // Act
            tag.AtualizarDescricao(novaDescricao);

            // Assert
            Assert.Equal(novaDescricao, tag.Descricao);
        }

        [Fact]
        public void AtualizarDescricao_NullNewDescricao_SetsDescricaoToEmptyString()
        {
            // Arrange
            var tag = new Tag("NomeValido", "DescricaoAntiga");

            // Act
            tag.AtualizarDescricao(null);

            // Assert
            Assert.Equal(string.Empty, tag.Descricao);
        }

        // 4. Ativar Method Tests
        [Fact]
        public void Ativar_WhenCalled_SetsAtivaToTrue()
        {
            // Arrange
            var tag = new Tag("NomeValido", "Descricao");
            tag.Desativar(); // Ensure it's false first

            // Act
            tag.Ativar();

            // Assert
            Assert.True(tag.Ativa);
        }
        
        [Fact]
        public void Ativar_WhenAlreadyActive_RemainsActive()
        {
            // Arrange
            var tag = new Tag("NomeValido", "Descricao"); // Ativa is true by default

            // Act
            tag.Ativar();

            // Assert
            Assert.True(tag.Ativa);
        }

        // 5. Desativar Method Tests
        [Fact]
        public void Desativar_WhenCalled_SetsAtivaToFalse()
        {
            // Arrange
            var tag = new Tag("NomeValido", "Descricao"); // Ativa is true by default

            // Act
            tag.Desativar();

            // Assert
            Assert.False(tag.Ativa);
        }
        
        [Fact]
        public void Desativar_WhenAlreadyInactive_RemainsInactive()
        {
            // Arrange
            var tag = new Tag("NomeValido", "Descricao");
            tag.Desativar(); // Set to false

            // Act
            tag.Desativar(); // Call again

            // Assert
            Assert.False(tag.Ativa);
        }
    }
}
