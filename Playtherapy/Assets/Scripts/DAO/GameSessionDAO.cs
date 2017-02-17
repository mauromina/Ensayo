﻿using UnityEngine;
using System.Collections;
using Npgsql;

public class GameSessionDAO
{
	public static bool InsertGameSession(GameSession game, string therapy_id)
    {
        bool exito = false;

        if (DBConnection.dbconn != null)
        {
            NpgsqlCommand dbcmd = DBConnection.dbconn.CreateCommand();

            try
            {
				string sql = string.Format("INSERT INTO start_gamesession (date, score, repetitions, time, level, minigame_id, therapy_id) VALUES ('{0}', {1}, {2}, {3}, '{4}', '{5}', {6});",
					game.Date, game.Score, game.Repetitions, game.Time, game.Level, game.Minigame_id, therapy_id);

                dbcmd.CommandText = sql;
                dbcmd.ExecuteNonQuery();

                //Debug.Log("");
                exito = true;
            }
            catch (NpgsqlException ex)
            {
                Debug.Log(ex.Message);
            }

            // clean up
            dbcmd.Dispose();
            dbcmd = null;
        }
        else
        {
            Debug.Log("Database connection not established");
        }

        return exito;
    }


	public static float GetRecord(GameSession game, string idPatient) {
		if (DBConnection.dbconn != null)
		{
			NpgsqlCommand dbcmd = DBConnection.dbconn.CreateCommand();

			string sql = ("SELECT ((score/repetitions::float) * 100)::int as record " +
				"FROM start_gamesession, start_therapysession " +
				"WHERE start_gamesession.therapy_id = start_therapysession.id " +
				"and (score/repetitions) = (SELECT max (score/repetitions) FROM start_gamesession, start_therapysession " +
				"WHERE level = " + game.Level + " and minigame_id = "+ game.Minigame_id + " and start_gamesession.therapy_id = start_therapysession.id " +
				"and patient_id = (SELECT id FROM patient_patient WHERE id_num = '" + idPatient + "')) " +
				"and level = " + game.Level + " and minigame_id = "+ game.Minigame_id +";");
			dbcmd.CommandText = sql;

			NpgsqlDataReader reader = dbcmd.ExecuteReader();
			if (reader.Read())
			{
				var record = (int)reader["record"];

				Debug.Log ("Record: " + record);
				// clean up
				reader.Close();
				reader = null;
				dbcmd.Dispose();
				dbcmd = null;

				return (float)record;
			}
			else
			{
				// clean up
				reader.Close();
				reader = null;
				dbcmd.Dispose();
				dbcmd = null;

				Debug.Log("Error de consulta o elemento no encontrado");
				return 0.0f;
			}            
		}
		else
		{
			Debug.Log("Database connection not established");
			return 0.0f;
		}
	}

    public static int GetLastGameSessionId(string id_therapy)
    {
        if (DBConnection.dbconn != null)
        {
            NpgsqlCommand dbcmd = DBConnection.dbconn.CreateCommand();

            string sql = ("SELECT max(id) as id FROM start_gamesession WHERE therapy_id = "+ id_therapy +";");
            dbcmd.CommandText = sql;

            NpgsqlDataReader reader = dbcmd.ExecuteReader();
            if (reader.Read())
            {
                //string numero_doc = (int)reader["id_num"];
                int id = (int)reader["id"];

                // clean up
                reader.Close();
                reader = null;
                dbcmd.Dispose();
                dbcmd = null;

                return id;
            }
            else
            {
                // clean up
                reader.Close();
                reader = null;
                dbcmd.Dispose();
                dbcmd = null;

                Debug.Log("Error de consulta o elemento no encontrado");
                return 0;
            }
        }
        else
        {
            Debug.Log("Database connection not established");
            return 0;
        }
    }
}
