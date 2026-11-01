/*
 * Comportamiento de las páginas de mantenimiento (Index de cada entidad):
 * - "Agregar": abre el formulario vacío con los valores iniciales.
 * - "Editar": abre el mismo formulario con los datos de la fila.
 * - "Detalle": muestra todos los campos del registro.
 */
(function () {
    'use strict';

    var page = document.getElementById('crud-page');
    if (!page) {
        return;
    }

    var fields = JSON.parse(page.getAttribute('data-fields'));
    var lookups = JSON.parse(page.getAttribute('data-lookups'));
    var singular = page.getAttribute('data-singular');

    var form = document.getElementById('crud-form');
    var formTitle = document.getElementById('crud-form-title');
    var detailsBody = document.getElementById('crud-details-body');
    var formModal = bootstrap.Modal.getOrCreateInstance(document.getElementById('crud-form-modal'));
    var detailsModal = bootstrap.Modal.getOrCreateInstance(document.getElementById('crud-details-modal'));

    function fillForm(values) {
        fields.forEach(function (field) {
            var input = form.querySelector('[data-field="' + field.name + '"]');
            if (!input) {
                return;
            }

            var value = values[field.name];
            if (field.kind === 'Checkbox') {
                input.checked = value === 'true';
            } else if (field.kind === 'Password') {
                input.value = '';
            } else {
                input.value = value === undefined || value === null ? '' : value;
            }
        });
    }

    function openForm(values, title) {
        form.reset();
        fillForm(values);
        formTitle.textContent = title;
        formModal.show();
    }

    function defaultValues() {
        var values = {};
        fields.forEach(function (field) {
            values[field.name] = field.defaultValue;
        });
        return values;
    }

    function displayValue(field, value) {
        if (value === undefined || value === null || value === '') {
            return '-';
        }
        if (field.kind === 'Checkbox') {
            return value === 'true' ? 'Sí' : 'No';
        }
        if (field.kind === 'Select' && lookups[field.name] && lookups[field.name][value]) {
            return lookups[field.name][value];
        }
        if (field.kind === 'DateTime') {
            return value.replace('T', ' ');
        }
        return value;
    }

    function showDetails(values) {
        detailsBody.innerHTML = '';
        fields.forEach(function (field) {
            if (!field.showInDetails) {
                return;
            }
            var term = document.createElement('dt');
            term.className = 'col-sm-4';
            term.textContent = field.label;

            var description = document.createElement('dd');
            description.className = 'col-sm-8';
            description.textContent = displayValue(field, values[field.name]);

            detailsBody.appendChild(term);
            detailsBody.appendChild(description);
        });
        detailsModal.show();
    }

    document.addEventListener('click', function (event) {
        var button = event.target.closest('[data-crud-action]');
        if (!button) {
            return;
        }

        var action = button.getAttribute('data-crud-action');
        var values = button.hasAttribute('data-entity') ? JSON.parse(button.getAttribute('data-entity')) : {};

        if (action === 'create') {
            openForm(defaultValues(), 'Agregar ' + singular);
        } else if (action === 'edit') {
            openForm(values, 'Editar ' + singular);
        } else if (action === 'details') {
            showDetails(values);
        }
    });

    // Las fechas obligatorias que se dejan vacías no se envían: el sistema usa la fecha actual.
    form.addEventListener('submit', function () {
        fields.forEach(function (field) {
            var input = form.querySelector('[data-field="' + field.name + '"]');
            if (input && field.kind === 'DateTime' && input.value === '' && !input.required) {
                input.disabled = true;
            }
        });
    });

    document.getElementById('crud-form-modal').addEventListener('hidden.bs.modal', function () {
        form.querySelectorAll('[disabled]').forEach(function (input) {
            input.disabled = false;
        });
    });
})();
