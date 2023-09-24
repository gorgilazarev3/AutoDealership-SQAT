//These tests only contain the source code for the Postman tests and cannot be run from this file
//, in order to run them you need to have Postman installed,
// create an API and set the collection with all of the methods specified
//in these files and run them in the order like they are declared here, in the Tests tab you need
//to copy the function body for the appropriate method
//Also, these tests use a incrementing Environment Variable for the Id's of the created and deleted Vehicles
//First you need to create an Environment and set the environment variable 'vehicleId' to the current
//Id that is in your testing database and after that set the collection to run in that environment
//After that open the API tab in the Postman dashboard, select the API, click on Tests and Automation
//and click Run on the collection and the tests will be run for you
//Make sure to have the application started on localhost if you use localhost for the testing or
//if you are using a host, make sure that the application is up and running there and the address is correct


//This function runs tests at the path /api/Vehicles with the GET Method
//Should return all available vehicles
function getAllVehiclesTest() {
    pm.test("Response returns status 200 - OK", function () {
        pm.response.to.have.status(200);
    });

    pm.test("At least one vehicle is returned", function () {
        var data = pm.response.json();
        pm.expect(data.length).greaterThan(0);
    });
}

//This function runs tests at the path /api/Vehicles/{{vehicleId}} with the GET Method
//Should return a specific vehicle, in my case with the Id 9 I have a Volkswagen Golf 7 Vehicle added
//and I'm testing the /api/Vehicles/9 endpoint with the GET method
function getSpecificVehicleTest() {
    pm.test("Response returns status 200 - OK", function () {
        pm.response.to.have.status(200);
    });

    var data = pm.response.json();

    pm.test("Only one vehicle is returned", function () {
        pm.expect(data).to.be.an('object');
    });

    pm.test("The correct vehicle is returned", function () {

        pm.expect(data['Model']).equals('Golf 7');
    });
}

//This function runs tests at the path /api/Vehicles/ with the POST Method
//Should create a new Vehicle, with the form data provided and return the created vehicle
//Below is an example of a form data I used to test this endpoint:
//-----------------------------------------------------
//Model: E63s
//BrandId: 10
//FuelType: 0
//Horsepower: 612
//BodyStyle: 0
//Year: 2018
//Mileage: 123000
//Color: Gray
//InteriorColor: White
//Transmission: { NumberSpeeds: 9, TransmissionType: 0 }
//------------------------------------------------------
function createANewVehicleTest() {
    pm.test("Response returns status 201 - Created", function () {
        pm.response.to.have.status(201);
    });

    var data = pm.response.json();

    pm.test("Response returns non empty vehicle", function () {
        pm.expect(data['Id']).is.not.null;
    });

}

//This function runs tests at the path /api/Vehicles/{{vehicleId}} with the PUT Method
//Should edit a Vehicle, with the form data provided and the path variable Id and return 204 - no content since it's defined like that in the API
//Below is an example of a form data I used to test this endpoint:
//-----------------------------------------------------
//Id: { { vehicleId } }
//Model: E53
//BrandId: 10
//FuelType: 0
//Horsepower: 550
//BodyStyle: 0
//Year: 2018
//Mileage: 321000
//Color: Black
//InteriorColor: Black
//Transmission: { NumberSpeeds: 9, TransmissionType: 0 }
//------------------------------------------------------
function editAVehicleTest() {
    pm.test("Response returns status 204 - No Content", function () {
        pm.response.to.have.status(204);
    });

}

//This function runs tests at the path /api/Vehicles/{{vehicleId}} with the DELETE Method
//Should return the deleted vehicle, in my case I delete the last vehicle added in the previous test
//that's why the tests should be run in this order
//after the deletion, the vehicleId variable is incremented, such that when a subsequent test is run
//the vehicleId represents the Id of the most recent added vehicle that is edited after and deleted
function deleteAVehicleTest() {
    pm.test("Response returns status 200 - OK", function () {
        pm.response.to.have.status(200);
    });

    var data = pm.response.json();
    let vehicleId = pm.environment.get("vehicleId");
    vehicleIdNum = Number.parseInt(vehicleId);
    pm.test("Response returns the deleted vehicle", function () {
        pm.expect(data['Id']).is.not.null;
        pm.expect(data['Id']).is.eq(vehicleIdNum);
    });
    vehicleId++ // increment by 1
    pm.environment.set("vehicleId", vehicleId);


}