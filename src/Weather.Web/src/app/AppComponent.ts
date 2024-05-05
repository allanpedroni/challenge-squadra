import { Component, OnInit } from '@angular/core';
import { WeatherForecastData } from '../models/weatherForecast.model';
import { WeatherForecastService } from './services/weather-forecast.service'

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  public city: string = '' || 'Belo Horizonte';
  public forecasts: WeatherForecastData[] = [];

  //constructor(private http: HttpClient) { }
  constructor(private weatherForecastService: WeatherForecastService) { }

  ngOnInit() {
    this.getWeatherForecasts();
  }

  getWeatherForecasts() {
    console.log(`searching for ${this.city}`);

    this.weatherForecastService.getWeatherForecast(this.city)
      .then(response => {
        this.forecasts = response;
        console.info(`Success getting weather forecast for ${this.city}.`);
      })
      .catch(error => {
        console.debug(`Error getting weather forecast for ${this.city}. Check the logs for more details.`)
        console.error(error);
      });
  }

  title = 'AngularProjectWeatherBoard';
}
