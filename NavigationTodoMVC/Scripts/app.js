(function (win) {
    'use strict';

	// The Navigation project makes Ajax requests for server-rendered panels
	// and sets them as the innerHTML of their respective DOM elements. It only
	// does this in browsers that support the HTML5 history api, indicated by
	// the presence of the refreshAjax object.
    if (!win.refreshAjax) {
    	return;
    }

	// A todo's title can be saved by clicking away. If that click results in
	// another request then we want to ignore the first response. If we didn't
	// ignore the first response then the second response would be cancelled by
	// the change to url caused by the first. The presence of this second request
    // is tracked by an edit flag.
    var edit = false;

    refreshAjax.navigating(function (req, resp) {
		// Track whether the latest Ajax request relates to editing a todo's title.
        edit = req.data && req.data.action === 'edit';
    });

    refreshAjax.updating(function (req, resp) {
    	// When a todo is added the DOM element to hold the server-rendered panel
		// doesn't exist yet so we must create the new list item to hold it.
        if (req.target.id === 'todo-form') {
        	var newTodoId = 0;

        	// The panel with the greatest id is for the newly added todo.
            for (var id in resp.panels) {
                var todoId = Number(id.slice(4));
                if (todoId && newTodoId < todoId)
                    newTodoId = todoId;
            }

			// Create the new list item to hold the server-rendered panel.
            if (newTodoId && !document.getElementById('todo' + newTodoId)) {
                document.getElementById('todo-list')
                    .insertAdjacentHTML('beforeend', '<li><span id="todo' + newTodoId + '" /></li>');
            }
        }

		// Only process the edit if it was the latest Ajax request.
        if (req.data && req.data.action === 'edit' && !edit)
            resp.panels = null;
    });

    refreshAjax.updated(function (req, resp) {
    	// When a todo is deleted, for example, no corresponding server-rendered 
    	// panel is returned and the todo element must be removed from the list.
    	if (req.target.getAttribute) {
			// Get the id of the todo being edited.
        	var todoId = req.target.getAttribute('data-todo');

			// Check for the absence of the todo's server-rendered panel.
            if (todoId && !resp.panels['todo' + todoId]) {
            	var todo = document.getElementById('todo' + todoId);

				//If the todo element exists in the list then we remove it.
                if (todo) {
                    var listItem = todo.parentNode;
                    listItem.parentNode.removeChild(listItem);
                }
            }
    	}

		// Initialise focus and listeners whenever the content's updated.
        initEdit(req);
    });

    function initEdit(req) {
    	edit = false;

		// Check if a todo's title is being edited.
    	var el = document.querySelector('#todo-list input[type="text"]');

    	if (el) {
    		// Focus to the todo being edited, setting the value moves the caret
			// to the end of the todo title.
            el.focus();
            el.value = el.value;

    		// On blur the todo's title must be saved or the todo deleted if the
    		// title's blank.
            el.addEventListener('blur', function (e) {
            	if (!edit) {
            		if (el.value.trim()) {
            			refreshAjax.navigate({ action: 'edit', Title: el.value }, el.form);
            		} else {
            			refreshAjax.navigate({ action: 'delete' }, el.form);
            		}
            	}
            });

			// On escape the todo's title must be reverted.
            el.addEventListener('keyup', function (e) {
            	if (!edit && e.keyCode === 27) {
            		refreshAjax.navigate({ action: 'edit', cancel: true }, el.form);
            	}
            });
    	} else if (req.target.id === 'todo-form') {
    		// Cater for bulk addition of todos by retaining focus in the add
    		// todo input when a todo's added.
            document.getElementById('new-todo').focus();
        }
    }

	// Initialise focus and listeners when the page is first loaded.
    initEdit({ target: { id: 'todo-form' } });
})(window);
