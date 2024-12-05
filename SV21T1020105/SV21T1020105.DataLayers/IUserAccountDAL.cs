﻿using SV21T1020105.DomainModels;

namespace SV21T1020105.DataLayers
{
    public interface IUserAccountDAL
    {
        /// <summary>
        /// Kiểm tra xem tên đăng nhập & mật khẩu có đúng hay không?
        /// Nếu đúng: trả về thông tin user, nếu sai: trả về null
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        UserAccount? Authorize(string username, string password);

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool ChangePassword(string username, string password, string newPassword);
    }
}
