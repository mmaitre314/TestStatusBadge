Test-status badge
===============

A trivial Web API returning SVG image status badges displaying AppVeyor test results: http://teststatusbadge.azurewebsites.net/api/status/{account}/{project}

For instance, with mmaitre314 as account and securestringcodegen as project:

[![Build status](https://ci.appveyor.com/api/projects/status/s08qgb4egku0pa3d?svg=true)
![Test status](http://teststatusbadge.azurewebsites.net/api/status/mmaitre314/securestringcodegen)]
(https://ci.appveyor.com/project/mmaitre314/securestringcodegen)

Using the markdown:

``` md
[![Build status](https://ci.appveyor.com/api/projects/status/s08qgb4egku0pa3d?svg=true)
![Test status](http://teststatusbadge.azurewebsites.net/api/status/mmaitre314/securestringcodegen)]
(https://ci.appveyor.com/project/mmaitre314/securestringcodegen)
```

