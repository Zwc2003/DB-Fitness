import axios from 'axios'
axios.defaults.headers.common['Authorization'] = 'Bearer '

const service = axios.create({
    baseURL: 'http://localhost:5173',//这个应该是把后端部署了
    timeout: 50000, // request timeout
    //withCredentials: true//携带cookie
    async: true,
    crossDomain: true,
})

export default service