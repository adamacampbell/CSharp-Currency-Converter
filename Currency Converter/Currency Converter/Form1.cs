using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
// Use post request
using System.Net;
using Newtonsoft.Json.Linq;
// Use JSON


namespace Currency_Converter
{
    public partial class Form1 : Form
    {
        // Global response val
        JObject data = new JObject() { };
        // Global exchange rate val
        double exRate = 0;
        // Global input val
        double input = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Get currency list
            // Create a request for the URL
            WebRequest request = WebRequest.Create(
                "https://v6.exchangerate-api.com/v6/23dc9198250b9ff553601f13/latest/GBP");
            // If required by the server, set the credentials
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response
            WebResponse response = request.GetResponse();
            // Display the status
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            // Get the stream containing content returned by the server
            // The using statement ensures that stream is closed outside of scope
            using (Stream dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access
                StreamReader reader = new StreamReader(dataStream);
                // Read the content
                string responseFromServer = reader.ReadToEnd();
                // Display the content
                Console.WriteLine(responseFromServer);
                // Set currency lists
                this.data = JObject.Parse(responseFromServer);
                Console.WriteLine(this.data);
                Console.WriteLine("--------");
                Console.WriteLine(this.data["conversion_rates"]);
                // Update currency selection box
                UpdateCurrencySelection();
            }
            // Close the response
            response.Close();
        }

        private void UpdateCurrencySelection()
        {
                // Temp value to hold currencies
                List<String> currencies = new List<string>();
                // Loop through each currency
                foreach (JProperty value in this.data["conversion_rates"])
                {
                    currencies.Add(value.Name);
                }
                // Update currency dropdown list values
                currencySelectionBox.DataSource = currencies;
        }

        private void currencySelectionBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get currency exchaneg rate
            string exRate = this.data["conversion_rates"][currencySelectionBox.SelectedItem].ToString();
            Console.WriteLine(exRate);
            Console.WriteLine("---------");
            // Convert exchange rate from string to double
            this.exRate = double.Parse(exRate);
            Console.WriteLine(this.exRate);
            Console.WriteLine("---------");
            // Update output
            updateOutput();
        }
        private void originalValue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Get original value as double
                double val = double.Parse(originalValue.Text);
                Console.WriteLine(val);
                Console.WriteLine("---------");
                // Update input val
                this.input = val;
                // Update output
                updateOutput();
            }
            catch (FormatException err)
            {
                Console.WriteLine(err);
            }
        }

        private void updateOutput()
        {
            try
            {
                // Calculate new value
                double newVal = this.input * this.exRate;
                Console.WriteLine(newVal);
                Console.WriteLine("---------");
                // Update output textbox value
                convertedValue.Text = newVal.ToString();
            } catch (FormatException err)
            {
                Console.WriteLine(err);
            }
        }

    }
}
