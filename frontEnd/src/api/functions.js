import request from '../api/request'


export function userLogin(params) {
    return request({
        url: '/user/login',
        method: 'post',
        data: {
            email: params.email,
            password: params.password
        }
    })
}

export function userSignup(params) {
    return request({
        url: '/user/register',
        method: 'post',
        data: {
            email: params.email,
            gender: params.gender,
            password: params.password,
            realName: params.realname,
        }
    })
}

export function userUpdate(params) {
    return request({
        url: '/user/update',
        method: 'put',
        data: {
            userId: params.userId,
            email: params.email,
            gender: params.gender,
            password: params.password,
            realName: params.realname,
            totalDuration: params.totalduration,
            totalTimeFit: params.totaltimefit,
            totalTimeClass: params.totaltimeclass
        }
    })
}

export function userGet(params) {
    return request({
        url: '/user',
        method: 'get',
        params: {
            userId: params.userid,
        }
    })
}