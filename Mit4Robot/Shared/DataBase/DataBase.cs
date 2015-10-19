using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SQLite;

namespace Shared.DataBase
{
	/// <summary>
	/// Database class.
	/// </summary>
	public class DataBase 
	{
		private SQLiteConnection connection;
		private static DataBase instance;

		/// Author:	Guy Spronck
		/// Date:	15-06-2015
		/// <summary>
		/// Returns the instance of DataBase
		/// </summary>
		public static DataBase Instance(){
			if (instance == null) {
				instance = new DataBase ();
			}
			return instance;
		}


		/// <summary>
		/// Gets the database path per platform
		/// </summary>
		/// <value>The database path.</value>
		public string DatabasePath {
			get { 
				var sqliteFilename = "localdb.db3";
				var path = "";
				#if __IOS__
				string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
				string libraryPath = Path.Combine (documentsPath, "..", "Library"); // Library folder
				path = Path.Combine(libraryPath, sqliteFilename);
				#else
				#if __ANDROID__
				string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
				path = Path.Combine(documentsPath, sqliteFilename);
				#else
				#if __WINPHONE__
				// WinPhone
				path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, sqliteFilename);;
				#endif
				#endif
				#endif
				return path;
			}
		}
			
		/// Author:	Guy Spronck
		/// Date:	15-06-2015
		/// <summary>
		/// Initializes a new instance of the <see cref="Shared.DataBase.DataBase"/> class.
		/// </summary>
		private DataBase()
		{
			connection = new SQLiteConnection (DatabasePath);

			// Create tables if not exist
			connection.CreateTable<Code>();
			connection.CreateTable<HighScore>();
			connection.CreateTable<Settings> ();
			connection.CreateTable<Achievement> ();
		}

		/// Author:	Guy Spronck
		/// Date:	15-06-2015
		/// <summary>
		/// Selects the first available model of given type in DB
		/// </summary>
		/// <returns>The first model available</returns>
		/// <typeparam name="T">Type of model</typeparam>
		public T SelectFirst<T>() where T:iTableDefault, new()
		{
			try{
				return connection.Table<T> ().First ();
			}catch(Exception){
				return default(T);
			}
		}

		/// Author:	Guy Spronck
		/// Date:	15-06-2015
		/// <summary>
		/// Selects all from database.
		/// </summary>
		/// <returns>List of given model</returns>
		/// <typeparam name="T">Type of model</typeparam>
		public List<T> SelectAll<T>() where T : iTableDefault, new()
		{
			try{
				return connection.Table<T> ().Select (row => row).ToList();
			}catch(Exception){
				return new List<T> ();
			}
		}

		/// Author:	Guy Spronck
		/// Date:	15-06-2015
		/// <summary>
		/// Selects the model by identifier.
		/// </summary>
		/// <returns>The selected model OR default.</returns>
		/// <param name="id">Identifier.</param>
		/// <typeparam name="T">Type of model</typeparam>
		public T SelectById<T>(int id)  where T : iTableDefault, new()
		{
			try{
				return connection.Table<T> ().First (x => x.ID == id);
			}catch(Exception){
				return default(T);
			}
		}

		/// Author:	Guy Spronck
		/// Date:	15-06-2015
		/// <summary>
		/// Insert the specified model.
		/// </summary>
		/// <param name="model">Model.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void Insert<T>(T model) where T : new()
		{
			connection.Insert (model);
		}

		/// Author:	Guy Spronck
		/// Date:	15-06-2015
		/// <summary>
		/// Update the specified model.
		/// </summary>
		/// <param name="model">Model.</param>
		/// <typeparam name="T">Type of model</typeparam>
		public void Update<T>(T model) where T: new(){
			connection.Update (model);
		}

		/// Author:	Guy Spronck
		/// Date:	15-06-2015
		/// <summary>
		/// Delete the specified model.
		/// </summary>
		/// <param name="model">Model.</param>
		/// <typeparam name="T">Type of model</typeparam>
		public void Delete<T>(T model) where T : new()
		{
			connection.Delete (model);
		}

		/// Author:	Guy Spronck
		/// Date:	15-06-2015
		/// <summary>
		/// Deletes the model by identifier.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <typeparam name="T">Type of model</typeparam>
		public void DeleteById<T>(int id) where T : new()
		{
			connection.Delete<T> (id);
		}
	}
}

