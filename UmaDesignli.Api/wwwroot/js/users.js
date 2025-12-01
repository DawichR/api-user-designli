// Check if user is logged in
const token = localStorage.getItem('token');

if (!token) {
    window.location.href = '/login';
}

// Decode JWT to get user name
function parseJwt(token) {
    try {
        const base64Url = token.split('.')[1];
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));
        return JSON.parse(jsonPayload);
    } catch (e) {
        return null;
    }
}

const decodedToken = parseJwt(token);
const userName = decodedToken?.name || decodedToken?.unique_name || 'User';
document.getElementById('currentUsername').textContent = userName;

// Fetch users
async function fetchUsers() {
    const loadingMessage = document.getElementById('loadingMessage');
    const errorMessage = document.getElementById('errorMessage');
    const usersTable = document.getElementById('usersTable');
    const noDataMessage = document.getElementById('noDataMessage');

    try {
        const response = await fetch('/api/users', {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            }
        });

        loadingMessage.style.display = 'none';

        if (response.status === 401) {
            localStorage.removeItem('token');
            localStorage.removeItem('username');
            window.location.href = '/login';
            return;
        }

        if (!response.ok) {
            throw new Error('Failed to fetch users');
        }

        const users = await response.json();

        if (users.length === 0) {
            noDataMessage.style.display = 'block';
        } else {
            const tbody = document.getElementById('usersTableBody');
            tbody.innerHTML = users.map(user => `
                <tr>
                    <td><span class="badge badge-primary">${user.id}</span></td>
                    <td>${user.username}</td>
                    <td>${user.email || '-'}</td>
                    <td>${user.name || '-'}</td>
                    <td>${user.lastName || '-'}</td>
                </tr>
            `).join('');
            usersTable.style.display = 'table';
        }
    } catch (error) {
        loadingMessage.style.display = 'none';
        errorMessage.textContent = 'Error loading users: ' + error.message;
        errorMessage.style.display = 'block';
    }
}

function logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('username');
    window.location.href = '/login';
}

// Load users on page load
fetchUsers();
