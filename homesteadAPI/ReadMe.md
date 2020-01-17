# Homesteader's Workbook API
An API that is designed to let end-users (client applications), create, organize, and plan gardens based on who they're trying to feed (or other reasons) 


## Data Currently stored in the Homesteader's Workbook
* There can be multiple named gardens
* A garden is a collection of Garden Plants
* A garden is related to a specific person who owns/plants it
* A garden plant gives info on what garden what plant is in, and how many individual plants are in the garden (e.g. a garden called "The Herb Garden" has 5 peppermint plants in it), expected and actual yield amounts
* Each Plant has a name, type of yield (lbs, fruit, bushels, etc.) and a Plant Group related to it
* The Plant Group collects plants, this is used for collecting varieties into groups (e.g. A Plant group is "Kale", and a plant in that group would be "Dinosaur Kale").

## Functionality
List of functionality this API supports. The exact API calls won't be listed, as those are still variable. Separate documentation for the API calls will/does exist.

### Completed

#### Garden Planning
* Plants have expected yield per plant that can be edited/updated
* Gardens are planned each year. Have garden plants/gardens able to be set to a specific growing time-frame
* Add/edit a plant to the workbook
* For each garden, can set actual yield per plant for future reference

#### Maximizing feeding people
n/a for now

### Need To Be Done

#### Garden Planning
* If no garden plants related to a plant in the workbook, allow the deletion of the plant
* Have gardens be owned by a person
* Add/edit garden plants to gardens that a person owns
* Can move garden plants to different gardens that a person owns
* Add/edit a plant group to the workbook
* add/move plants into plant groups in the workbook

#### Maximizing feeding people
* People can have a collection of "people-they're-feeding"
* Each person who's being fed has a set of nutritional requirements that need to be met
* The person who creates/owns the gardens also has a set of nutritional requirements
* A person can have a set of plants that they own the seeds/seedlings for
* For a given person and "people they're feeding", generate a garden that most closely matches/exceeds the nutritional requirements of the group, preferably using the seeds they already have first


## Technical To-do's
### Completed
* Create Git Repo and basic .gitignore file
* Create basic data model and set-up to create/seed database on startup of dev site
* Create readme with functionality list and tech TO-DOs
* set-up testing framework

### Need to be done
* Create auto-generated API Documentation
* set-up non-dev SQL database and configurations
* set-up/configure deploy to a test environment not on my local system so others can test it
* Set-up log in to incorporate google account login