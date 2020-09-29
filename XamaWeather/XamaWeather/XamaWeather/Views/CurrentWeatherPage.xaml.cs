using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamaWeather.Helper;
using XamaWeather.Models;

namespace XamaWeather.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrentWeatherPage : ContentPage
    {
        public CurrentWeatherPage()
        {
            InitializeComponent();
            GetWeatherInfo();
        }

        private String Location = "Netherlands";

        private async void GetWeatherInfo()
        {
            var url = $"http://api.openweathermap.org/data/2.5/weather?q={Location}&appid=ab53a4c09b766fce560aaa627b0265d5&units=metric";

            var result = await ApiCaller.GetApiResponse(url);

            if(result.IsSuccessful)
            {
                try
                {
                    var weatherInfo = JsonConvert.DeserializeObject<WeatherInfo>(result.JsonResponse);
                    descriptionText.Text = weatherInfo.weather[0].description.ToUpper();
                    iconImg.Source = $"w{weatherInfo.weather[0].icon}";
                    cityText.Text = weatherInfo.name.ToUpper();
                    temperatureText.Text = weatherInfo.main.temp.ToString("0");
                    humidityText.Text = $"{weatherInfo.main.humidity}%";
                    pressureText.Text = $"{weatherInfo.main.pressure} hpa";
                    windText.Text = $"{weatherInfo.wind.speed} m/s";
                    cloudinessText.Text = $"{weatherInfo.clouds.all}%";

                    var dateTime = new DateTime().ToUniversalTime().AddSeconds(weatherInfo.dt);
                    dateText.Text = dateTime.ToString("dddd, MMM dd").ToUpper();

                    GetForcastInfo();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Weather Information!", ex.Message, "OK");
                }
            }
            else
            {
                await DisplayAlert("Weather Information!", "No weather information.", "OK");
            }
        }

        private async void GetForcastInfo()
        {
            var url = $"http://api.openweathermap.org/data/2.5/weather?q={Location}&appid=ab53a4c09b766fce560aaa627b0265d5&units=metric";

            var result = await ApiCaller.GetApiResponse(url);

            if (result.IsSuccessful)
            {
                try
                {
                    var forecastInfo = JsonConvert.DeserializeObject<ForecastInfo>(result.JsonResponse);

                    List<List> allList = new List<List>();

                    foreach (var list in forecastInfo.list)
                    {
                        //var date = DateTime.ParseExact(list.dt_txt, "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);
                        var date = DateTime.Parse(list.dt_txt);

                        if (date > DateTime.Now && date.Hour == 0 && date.Minute == 0 && date.Second == 0)
                        {
                            allList.Add(list);
                        }
                    }

                    dayOneText.Text = DateTime.Parse(allList[0].dt_txt).ToString("dddd");
                    dateOneText.Text = DateTime.Parse(allList[0].dt_txt).ToString("dd MMM");
                    iconOneImg.Source = $"w{allList[0].weather[0].icon}";
                    tempOneText.Text = allList[0].main.temp.ToString("0");

                    dayTwoText.Text = DateTime.Parse(allList[1].dt_txt).ToString("dddd");
                    dateTwoText.Text = DateTime.Parse(allList[1].dt_txt).ToString("dd MMM");
                    iconTwoImg.Source = $"w{allList[1].weather[0].icon}";
                    tempTwoText.Text = allList[1].main.temp.ToString("0");

                    dayThreeText.Text = DateTime.Parse(allList[2].dt_txt).ToString("dddd");
                    dateThreeText.Text = DateTime.Parse(allList[2].dt_txt).ToString("dd MMM");
                    iconThreeImg.Source = $"w{allList[2].weather[0].icon}";
                    tempThreeText.Text = allList[2].main.temp.ToString("0");

                    dayFourText.Text = DateTime.Parse(allList[3].dt_txt).ToString("dddd");
                    dateFourText.Text = DateTime.Parse(allList[3].dt_txt).ToString("dd MMM");
                    iconFourImg.Source = $"w{allList[3].weather[0].icon}";
                    tempFourText.Text = allList[3].main.temp.ToString("0");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Weather Information!", ex.Message, "OK");
                }
            }
            else
            {
                await DisplayAlert("Weather Information!", "No forecast information.", "OK");
            }
        }
    }
}