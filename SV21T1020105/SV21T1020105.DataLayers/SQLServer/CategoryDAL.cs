﻿using Dapper;
using SV21T1020105.DomainModels;
using System.Data;

namespace SV21T1020105.DataLayers.SQLServer
{
    public class CategoryDAL : BaseDAL, ICommonDAL<Category>
    {
        public CategoryDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Category data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Categories where CategoryName = @CategoryName)
                                select -1;
                            else
                                begin
                                    insert into Categories(CategoryName, Description, Photo)
                                    values (@CategoryName, @Description, @Photo);
                                    select SCOPE_IDENTITY();
                                end";
                var parameters = new
                {
                    CategoryName = data.CategoryName ?? "",
                    Description = data.Description ?? "",
                    Photo = data.Photo ?? ""
                };
                id = connection.ExecuteScalar<int>(sql, parameters, commandType: CommandType.Text);
                //Thực thi câu lệnh
                connection.Close();
            }
            return id;
        }

        public int Count(string searchValue = "")
        {
            int count = 0;
            searchValue = $"%{searchValue}%";
            using (var connection = OpenConnection())
            {
                var sql = @"select count(*)
                            from Categories
                            where (CategoryName like @searchValue)";
                var parameters = new
                {
                    searchValue = searchValue,
                };
                count = connection.ExecuteScalar<int>(sql, parameters, commandType: CommandType.Text);
            }

            return count;
        }

        public bool Delete(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from Categories where CategoryID = @CategoryID";
                var parameters = new
                {
                    CategoryID = id
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public Category? Get(int id)
        {
            Category? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select * from Categories where CategoryID = @CategoryID";
                var parameters = new
                {
                    CategoryID = id
                };
                data = connection.QueryFirstOrDefault<Category>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool InUsed(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists (select * from Products where CategoryID = @CategoryID)
	                            select 1
                            else
	                            select 0";
                var parameters = new
                {
                    CategoryID = id
                };
                result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return result;
        }

        public List<Category> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Category> data = new List<Category>();
            searchValue = $"%{searchValue}%";
            using (var connection = OpenConnection())
            {
                var sql = @"select * 
                            from(
                                select *,
                                    row_number() over(order by CategoryName) as RowNumber
                                from Categories
                                where (CategoryName like @searchValue)
                                ) as t
                            where (@pageSize = 0)
                                   or(RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize)
                            order by RowNumber";
                var parameters = new
                {
                    page = page,
                    pageSize = pageSize,
                    searchValue = searchValue
                };
                data = connection.Query<Category>(sql:sql, param:parameters, commandType: CommandType.Text).ToList();
            }
            return data;
        }

        public bool Update(Category data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if not exists(select * from Categories where CategoryID <> @CategoryID and CategoryName = @CategoryName)
                                begin
                                    update Categories
                                    set CategoryName = @CategoryName,
	                                    Description = @Description,
                                        Photo = @Photo
                                    where CategoryID = @CategoryID
                                end";
                var parameters = new
                {
                    CategoryID = data.CategoryID,
                    CategoryName = data.CategoryName,
                    Description = data.Description ?? "",
                    Photo = data.Photo ?? ""
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
    }
}
