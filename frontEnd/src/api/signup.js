import request from '../api/request'
export function signup(params) {
    return request({
        url: '/user/register',
        method: 'post',
        data: {
            realName: params.name,
            gender: params.sex,
            password: params.password,
            email: params.email,
            verifyCode: params.verifyCode
        }
    })
}

export function code(mail) {
    return request({
        url: '/mail/verifyCode',
        method: 'post',
        data: {
            mailBox: mail,
        }
    })
}