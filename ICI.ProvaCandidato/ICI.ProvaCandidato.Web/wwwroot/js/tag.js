$(document).ready(function () {
    console.log('tag.js loaded successfully');

    // Inicializa validação do formulário
    function initializeForm() {
        $('#formTag').validate({
            rules: {
                Nome: {
                    required: true,
                    maxlength: 50
                },
                Descricao: {
                    maxlength: 200
                }
            },
            messages: {
                Nome: {
                    required: "Nome é obrigatório",
                    maxlength: "Nome não pode exceder 50 caracteres"
                },
                Descricao: {
                    maxlength: "Descrição não pode exceder 200 caracteres"
                }
            },
            errorPlacement: function (error, element) {
                error.addClass("text-danger");
                error.insertAfter(element);
            },
            submitHandler: function (form) {
                console.log('Form submitted');
                const formData = {
                    Id: $('#Id').val(),
                    Nome: $('#Nome').val(),
                    Descricao: $('#Descricao').val(),
                    Ativa: $('#Ativa').length ? $('#Ativa').is(':checked') : false
                };

                const url =  formData.Id > 0 ? '/Tag/Edit' : '/Tag/Create';


                $.ajax({
                    url: url,
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(formData),
                    success: function (response) {
                        console.log('AJAX success:', response);
                        if (response.sucesso) {
                            $('#modalFormulario').modal('hide');
                            location.reload();
                        } else {
                            exibirErros(response.erros || [response.mensagem]);
                        }
                    },
                    error: function (xhr, status, error) {
                        console.error('AJAX error:', status, error);
                        alert('Erro ao processar requisição: ' + error);
                    }
                });
            }
        });
    }

    // Nova Tag
    $('#btnNovaTag').click(function () {
        console.log('Nova Tag button clicked');
        $.get('/Tag/Create', function (data) {
            console.log('Create partial view loaded');
            $('#modalFormulario .modal-body').html(data);
            initializeForm();
            $('#modalFormulario').modal('show');
        }).fail(function (xhr, status, error) {
            console.error('Error loading create form:', status, error);
            alert('Erro ao carregar formulário: ' + error);
        });
    });

    // Editar
    $('.btn-editar').click(function () {
        console.log('Edit button clicked');
        const id = $(this).data('id');
        $.get('/Tag/Edit/' + id, function (data) {
            console.log('Edit partial view loaded');
            $('#modalFormulario .modal-body').html(data);
            initializeForm();
            $('#modalFormulario').modal('show');
        }).fail(function (xhr, status, error) {
            console.error('Error loading edit form:', status, error);
            alert('Erro ao carregar formulário: ' + error);
        });
    });

    // Excluir
    $('.btn-excluir').click(function () {
        console.log('Delete button clicked');
        const id = $(this).data('id');
        $.get('/Tag/Delete/' + id, function (data) {
            console.log('Delete partial view loaded');
            $('#modalFormulario .modal-body').html(data);
            $('#modalFormulario').modal('show');
        }).fail(function (xhr, status, error) {
            console.error('Error loading delete form:', status, error);
            alert('Erro ao carregar formulário de exclusão: ' + error);
        });
    });

    // Confirmar Exclusão
    $(document).on('click', '.btn-excluir-confirmar', function () {
        console.log('Confirm delete button clicked');
        const id = $(this).data('id');
        if (!id || id <= 0) {
            alert('ID inválido para exclusão');
            return;
        }
        $.ajax({
            url: '/Tag/Delete',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ id: parseInt(id) }),
            success: function (response) {
                console.log('Delete AJAX success:', response);
                if (response.sucesso) {
                    $('#modalFormulario').modal('hide');
                    location.reload();
                } else {
                    alert(response.mensagem || 'Erro ao excluir tag');
                }
            },
            error: function (xhr, status, error) {
                console.error('Delete AJAX error:', status, error);
                alert('Erro ao processar requisição: ' + error);
            }
        });
    });

    // Exibir Erros
    function exibirErros(erros) {
        $('.text-danger').remove();
        for (const campo in erros) {
            const mensagens = Array.isArray(erros[campo]) ? erros[campo] : [erros[campo]];
            const elemento = $(`#${campo}`);
            if (elemento.length) {
                mensagens.forEach(function (mensagem) {
                    elemento.after(`<span class="text-danger">${mensagem}</span>`);
                });
            }
        }
    }
});