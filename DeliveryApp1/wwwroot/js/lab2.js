const uri = 'api/Users';
let users = [];

function getUsers() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayUsers(data))
        .catch(error => console.error('Unable to get users.', error));
}

function addUser() {
    const addLoginTextbox = document.getElementById('add-login');
    const addPhoneTextbox = document.getElementById('add-phoneNumber');
    const addDateTextbox = document.getElementById('add-dateBirth');

    const user = {
        login: addLoginTextbox.value.trim(),
        phoneNumber: addPhoneTextbox.value.trim(),
        dateBirth: addDateTextbox.value.trim(),
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(user)
    })
        .then(response => response.json())
        .then(() => {
            getUsers();
            addLoginTextbox.value = '';
            addPhoneTextbox.value = '';
            addDateTextbox.value = '';
        })
        .catch(error => console.error('Unable to add user.', error));
}

function deleteUser(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getUsers())
        .catch(error => console.error('Unable to delete user.', error));
}

function displayEditForm(id) {
    const user = users.find(user => user.id === id);

    document.getElementById('edit-id').value = user.id;
    document.getElementById('edit-login').value = user.login;
    document.getElementById('edit-phoneNumber').value = user.phoneNumber;
    document.getElementById('edit-dateBirth').value = user.dateBirth;
    document.getElementById('editForm').style.display = 'block';
}

function updateUser() {
    const userId = document.getElementById('edit-id').value;
    const user = {
        id: parseInt(userId, 10),
        login: document.getElementById('edit-login').value.trim(),
        phoneNumber: document.getElementById('edit-phoneNumber').value.trim(),
        dateBirth: document.getElementById('edit-dateBirth').value.trim()
    };

    fetch(`${uri}/${userId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(user)
    })
        .then(() => getUsers())
        .catch(error => console.error('Unable to update user.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}


function _displayUsers(data) {
    const tBody = document.getElementById('users');
    tBody.innerHTML = '';


    const button = document.createElement('button');

    data.forEach(user => {
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${user.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteUser(${user.id})`);

        let tr = tBody.insertRow();

        
        let td1 = tr.insertCell(0);
        let textNode = document.createTextNode(user.login);
        td1.appendChild(textNode);

        let td2 = tr.insertCell(1);
        let textNodePhone = document.createTextNode(user.phoneNumber);
        td2.appendChild(textNodePhone);

        let td3 = tr.insertCell(2);
        let textNodeDate = document.createTextNode(user.dateBirth);
        td3.appendChild(textNodeDate);

        let td4 = tr.insertCell(3);
        td4.appendChild(editButton);

        let td5 = tr.insertCell(4);
        td5.appendChild(deleteButton);
    });

    users = data;
}