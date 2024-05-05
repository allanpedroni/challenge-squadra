import { Injectable } from '@angular/core';
//import { HttpClient } from '@angular/common/http';
import axios from 'axios';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WeatherForecastService {

  private weatherApiUrl = 'http://localhost:5072/api';

  constructor() {
  }

  getWeatherForecast(city: string): Promise<any> {
    var url = `${this.weatherApiUrl}/forecast?CityName=${city}`;
    console.log(url);
    return axios.get(url)
      .then(response => response.data)
      .catch(error => console.error('There was an error!', error));
  }
}
