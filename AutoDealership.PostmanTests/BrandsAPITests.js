//These tests only contain the source code for the Postman tests and cannot be run from this file
//, in order to run them you need to have Postman installed,
// create an API and set the collection with all of the methods specified
//in these files and run them in the order like they are declared here, in the Tests tab you need
//to copy the function body for the appropriate method
//Also, these tests use a incrementing Environment Variable for the Id's of the created and deleted brands
//First you need to create an Environment and set the environment variable 'brandId' to the current
//Id that is in your testing database and after that set the collection to run in that environment
//After that open the API tab in the Postman dashboard, select the API, click on Tests and Automation
//and click Run on the collection and the tests will be run for you
//Make sure to have the application started on localhost if you use localhost for the testing or
//if you are using a host, make sure that the application is up and running there and the address is correct


//This function runs tests at the path /api/Brands with the GET Method
//Should return all available brands
function getAllBrandsTest() {
    pm.test("Response returns status 200 - OK", function () {
        pm.response.to.have.status(200);
    });

    pm.test("At least one brand is returned", function () {
        var data = pm.response.json();
        pm.expect(data.length).greaterThan(0);
    });
}

//This function runs tests at the path /api/Brands/{{brandId}} with the GET Method
//Should return a specific brand, in my case with the Id 5 I have the Volkswagen brand added
//and I'm testing the /api/Brands/5 endpoint with the GET method
function getSpecificBrandTest() {
    pm.test("Response returns status 200 - OK", function () {
        pm.response.to.have.status(200);
    });

    var data = pm.response.json();

    pm.test("Only one brand is returned", function () {
        pm.expect(data).to.be.an('object');
    });

    pm.test("The correct brand is returned", function () {

        pm.expect(data['Name']).equals('Volkswagen');
    });


}

//This function runs tests at the path /api/Brands/ with the POST Method
//Should create a new Brand, with the form data provided and return the created brand
//Below is an example of a form data I used to test this endpoint:
//-----------------------------------------------------
//Name: Skoda
//LogoURL: SKODA_LOGO
//------------------------------------------------------
function createANewBrandTest() {
    pm.test("Response returns status 201 - Created", function () {
        pm.response.to.have.status(201);
    });

    var data = pm.response.json();

    pm.test("Response returns non empty brand", function () {
        pm.expect(data['Id']).is.not.null;
    });

}

//This function runs tests at the path /api/Brands/{{brandId}} with the PUT Method
//Should edit a Brand, with the form data provided and the path variable Id and return 204 - no content since it's defined like that in the API
//Below is an example of a form data I used to test this endpoint:
//-----------------------------------------------------
//Name: Skoda_Changed
//LogoURL: SKODA_LOGO_CHANGED
//Id: { { brandId } }
//------------------------------------------------------
function editABrandTest() {
    pm.test("Response returns status 204 - No Content", function () {
        pm.response.to.have.status(204);
    });

}

//This function runs tests at the path /api/Brands/{{brandId}} with the DELETE Method
//Should return the deleted brand, in my case I delete the last brand added in the previous test
//that's why the tests should be run in this order
//after the deletion, the brandId variable is incremented, such that when a subsequent test is run
//the brandId represents the Id of the most recent added brand that is edited after and deleted
function deleteABrandTest() {
    pm.test("Response returns status 200 - OK", function () {
        pm.response.to.have.status(200);
    });

    var data = pm.response.json();
    let brandId = pm.environment.get("brandId");
    brandIdNum = Number.parseInt(brandId);
    pm.test("Response returns the deleted brand", function () {
        pm.expect(data['Id']).is.not.null;
        pm.expect(data['Id']).is.eq(brandIdNum);
    });
    brandId++ // increment by 1
    pm.environment.set("brandId", brandId);




}