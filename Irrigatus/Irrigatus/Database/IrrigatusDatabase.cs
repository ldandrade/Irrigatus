﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Irrigatus.Model;

namespace Irrigatus.Database
{
    public class IrrigatusDatabase
    {
        SQLiteAsyncConnection database;

        public IrrigatusDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<SensorMeasure>().Wait();
            database.CreateTableAsync<WateringEvent>().Wait();
            database.CreateTableAsync<WateringStation>().Wait();
        }

        public Task<List<WateringStation>> GetWateringStationsAsync()
        {
            return database.Table<WateringStation>().ToListAsync();
        }

        public Task<WateringStation> GetWateringStationAsync(string guid)
        {
            return database.Table<WateringStation>().Where(i => i.guid == guid).FirstOrDefaultAsync();
        }

        public async Task<int> SaveWateringStationAsync(WateringStation wateringStation)
        {
            WateringStation existingStation = await database.Table<WateringStation>().Where(i => i.guid == wateringStation.guid).FirstOrDefaultAsync();
            if (existingStation != null)
            {
                return database.UpdateAsync(wateringStation).Result;
            }
            else
            {
                return database.InsertAsync(wateringStation).Result;
            }
        }
    }
}
