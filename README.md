![.net](https://img.shields.io/badge/.NET-5C2D91?style=flat-square&logo=.net&logoColor=white)
![GraphQL](https://img.shields.io/badge/-GraphQL-E10098?style=flat-square&logo=graphql&logoColor=white)

![GitHub](https://img.shields.io/github/license/yannickcpnv/PokeCharts-Backend?style=flat-square)
![GitHub contributors](https://img.shields.io/github/contributors/yannickcpnv/PokeCharts-Backend?style=flat-square)
![GitHub issues](https://img.shields.io/github/issues-raw/yannickcpnv/PokeCharts-Backend?style=flat-square)
![GitHub closed issues](https://img.shields.io/github/issues-closed-raw/yannickcpnv/PokeCharts-Backend?style=flat-square)


# PokeCharts-Backend

This project is the backend of the PokeCharts application. It is developped in C# .NET
Our data are coming from the PokeAPI.

## Table of Contents

1. [Setting up dev](#setting-up-dev)
    1. [Requirements](#requirements)
    2. [Clone repository](#clone-repository)
    3. [Tools](#tools)
    4. [Run PokeCharts](#run-pokecharts)
    5. [Run Unit Tests](#run-unit-tests)
2. [Branches strategy](#branches-strategy)
3. [Contributing](#contributing)
4. [License](#licence)


## Setting up dev

### Requirements

| Version |  Description  | 
|---|---|
| Dotnet 7.0.101 | For code execution  |


### Clone repository

```sh
git clone git@github.com:yannickcpnv/PokeCharts-Backend.git
cd PokeCharts-Backend
git switch develop
```

### Tools

We use the [console](https://beta.pokeapi.co/graphql/console/) provided by PokeAPI to test our graphql queries.

### Run PokeCharts

> Build the firstime you download the repository
```shell
dotnet build
dotnet run
```

### Run Tests

To simply check that all tests pass
```shell
dotnet test
```

To get the test details
```shell
dotnet test --logger:"console;verbosity=detailed"
```

## Branches strategy

Refer to the workflow [GitFlow](https://www.atlassian.com/git/tutorials/comparing-workflows/gitflow-workflow).

GitFlow installation:
- [Windows](https://git-scm.com/download/win)
- [Linux/Ubuntu](https://howtoinstall.co/en/git-flow)

Main branches description:

| Branches  | Description |
|---|---|
| main/ | stores the official release history  |
| develop/ | serves as an integration branch for features |
| feature/| fork of `develop/` for a new feature|
|release/|fork of `develop/`, once the release is ready, it should be merge to `main`|


The convention name for `feature/` is `Snake Case`
Example: `feature/my_new_feature`

## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are greatly appreciated.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement". Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (git checkout -b feature/amazing_feature)
3. Commit your Changes (git commit -m 'Add some amazing_feature')
4. Push to the Branch (git push origin feature/amazing_feature)
5. Open a Pull Request

## Licence

Distributed under the MIT License. See LICENSE for more information.
