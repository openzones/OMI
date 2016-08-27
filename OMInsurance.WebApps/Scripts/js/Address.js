var AddressProcessor = AddressProcessor || {
    bindRegion: function (addressContainerId) {
        $("#" + addressContainerId + " .regionFIASLabel").autocomplete({
            dataType: 'json',
            source: function (request, response) {
                var autocompleteUrl = window.location.origin + '/FIAS/GetRegions?name=' + encodeURIComponent(request.term);
                $.ajax({
                    url: autocompleteUrl,
                    type: 'GET',
                    cache: false,
                    dataType: 'json',
                    success: function (json) {
                        response($.map(json, function (data, id) {
                            return {
                                label: data.Text,
                                value: data.Value
                            };
                        }));
                    },
                    error: function (xmlHttpRequest, textStatus, errorThrown) {
                        console.log('some error occured', textStatus, errorThrown);
                    }
                });
            },
            minLength: 0,
            change: function( event, ui ) {
                if (!ui.item) {
                    $("#" + addressContainerId + " .regionFIASLabel").val("");
                    $("#" + addressContainerId + " .regionFIASValue").val("");
                }
                    AddressProcessor.buildFullAddress(addressContainerId);
            },
            select: function (event, ui) {
                $("#" + addressContainerId + " .regionFIASLabel").val(ui.item.label);
                $("#" + addressContainerId + " .regionFIASValue").val(ui.item.value);
                return false;
            },
            focus: function (event, ui) {
                event.preventDefault();
                $(this).val(ui.item.label);
            }
        });
    },
    bindArea: function (addressContainerId) {
        $("#" + addressContainerId + " .areaFIASLabel").autocomplete({
            dataType: 'json',
            source: function (request, response) {
                var regionId = $("#" + addressContainerId + " .regionFIASValue").val();
                var autocompleteUrl = window.location.origin + '/FIAS/GetAreas?name=' + encodeURIComponent(request.term) + '&regionId=' + regionId;
                $.ajax({
                    url: autocompleteUrl,
                    type: 'GET',
                    cache: false,
                    dataType: 'json',
                    success: function (json) {
                        response($.map(json, function (data, id) {
                            return {
                                label: data.Text,
                                value: data.Value
                            };
                        }));
                    },
                    error: function (xmlHttpRequest, textStatus, errorThrown) {
                        console.log('some error occured', textStatus, errorThrown);
                    }
                });
            },
            change: function (event, ui) {
                if (!ui.item) {
                    $("#" + addressContainerId + " .areaFIASLabel").val("");
                    $("#" + addressContainerId + " .areaFIASValue").val("");
                }
                AddressProcessor.buildFullAddress(addressContainerId);
            },
            minLength: 0,
            select: function (event, ui) {
                $("#" + addressContainerId + " .areaFIASLabel").val(ui.item.label);
                $("#" + addressContainerId + " .areaFIASValue").val(ui.item.value);
                return false;
            },
            focus: function (event, ui) {
                event.preventDefault();
                $(this).val(ui.item.label);
            }
        });
    },
    bindLocality: function (addressContainerId) {
        $("#" + addressContainerId + " .localityFIASLabel").autocomplete({
            dataType: 'json',
            source: function (request, response) {
                var regionId = $("#" + addressContainerId + " .regionFIASValue").val();
                var areaId = $("#" + addressContainerId + " .areaFIASValue").val();
                var autocompleteUrl = window.location.origin + '/FIAS/GetLocalities?name='
                    + encodeURIComponent(request.term) + '&regionId=' + regionId + '&areaId=' + areaId;
                $.ajax({
                    url: autocompleteUrl,
                    type: 'GET',
                    cache: false,
                    dataType: 'json',
                    success: function (json) {
                        response($.map(json, function (data, id) {
                            return {
                                label: data.Text,
                                value: data.Value
                            };
                        }));
                    },
                    error: function (xmlHttpRequest, textStatus, errorThrown) {
                        console.log('some error occured', textStatus, errorThrown);
                    }
                });
            },
            minLength: 0,
            change: function (event, ui) {
                if (!ui.item) {
                    $("#" + addressContainerId + " .localityFIASLabel").val("");
                    $("#" + addressContainerId + " .localityFIASValue").val("");
                }
                AddressProcessor.buildFullAddress(addressContainerId);
            },
            select: function (event, ui) {
                $("#" + addressContainerId + " .localityFIASLabel").val(ui.item.label);
                $("#" + addressContainerId + " .localityFIASValue").val(ui.item.value);
                return false;
            },
            focus: function (event, ui) {
                event.preventDefault();
                $(this).val(ui.item.label);
            }
        });
    },
    bindCity: function (addressContainerId) {
        $("#" + addressContainerId + " .cityFIASLabel").autocomplete({
            dataType: 'json',
            source: function (request, response) {
                var regionId = $("#" + addressContainerId + " .regionFIASValue").val();
                var areaId = $("#" + addressContainerId + " .areaFIASValue").val();
                var autocompleteUrl = window.location.origin + '/FIAS/GetCities?name='
                    + encodeURIComponent(request.term) + '&regionId=' + regionId + '&areaId=' + areaId;
                $.ajax({
                    url: autocompleteUrl,
                    type: 'GET',
                    cache: false,
                    dataType: 'json',
                    success: function (json) {
                        response($.map(json, function (data, id) {
                            return {
                                label: data.Text,
                                value: data.Value
                            };
                        }));
                    },
                    error: function (xmlHttpRequest, textStatus, errorThrown) {
                        console.log('some error occured', textStatus, errorThrown);
                    }
                });
            },
            minLength: 0,
            change: function (event, ui) {
                if (!ui.item) {
                    $("#" + addressContainerId + " .cityFIASLabel").val("");
                    $("#" + addressContainerId + " .cityFIASValue").val("");
                }
                AddressProcessor.buildFullAddress(addressContainerId);
            },
            select: function (event, ui) {
                $("#" + addressContainerId + " .cityFIASLabel").val(ui.item.label);
                $("#" + addressContainerId + " .cityFIASValue").val(ui.item.value);
                return false;
            },
            focus: function (event, ui) {
                event.preventDefault();
                $(this).val(ui.item.label);
            }
        });
    },
    bindStreet: function (addressContainerId) {
        $("#" + addressContainerId + " .streetFIASLabel").autocomplete({
            dataType: 'json',
            source: function (request, response) {
                var regionId = $("#" + addressContainerId + " .regionFIASValue").val();
                var autocompleteUrl = window.location.origin + '/FIAS/GetStreets?name='
                    + encodeURIComponent(request.term) + '&regionId=' + regionId;
                $.ajax({
                    url: autocompleteUrl,
                    type: 'GET',
                    cache: false,
                    dataType: 'json',
                    success: function (json) {
                        response($.map(json, function (data, id) {
                            return {
                                label: data.Text,
                                value: data.Value
                            };
                        }));
                    },
                    error: function (xmlHttpRequest, textStatus, errorThrown) {
                        console.log('some error occured', textStatus, errorThrown);
                    }
                });
            },
            minLength: 5,
            select: function (event, ui) {
                $("#" + addressContainerId + " .streetFIASLabel").val(ui.item.label);
                $("#" + addressContainerId + " .streetFIASValue").val(ui.item.value);
                return false;
            },
            change: function (event, ui) {
                var regionId = $("#" + addressContainerId + " .regionFIASValue").val();
                if (!ui.item && regionId === "0c5b2444-70a0-4932-980c-b4dc0d3f02b5") {
                    $("#" + addressContainerId + " .streetFIASLabel").val("");
                    $("#" + addressContainerId + " .streetFIASValue").val("");
                }
                AddressProcessor.buildFullAddress(addressContainerId);
            },
            focus: function (event, ui) {
                event.preventDefault();
                $(this).val(ui.item.label);
            }
        });
    },
    clearRegion: function (addressContainerId) {
        $("#" + addressContainerId + " .regionFIASLabel").val("");
        $("#" + addressContainerId + " .regionFIASValue").val("");
    },
    clearArea: function (addressContainerId) {
        $("#" + addressContainerId + " .areaFIASValue").val("");
        $("#" + addressContainerId + " .areaFIASLabel").val("");
    },
    clearLocality: function (addressContainerId) {
        $("#" + addressContainerId + " .localityFIASLabel").val("");
        $("#" + addressContainerId + " .localityFIASValue").val("");
    },
    clearStreet: function (addressContainerId) {
        $("#" + addressContainerId + " .streetFIASLabel").val("");
        $("#" + addressContainerId + " .streetFIASValue").val("");
    },
    buildFullAddress: function (addressContainerId) {
        var region = $("#" + addressContainerId + " .regionFIASLabel").val();
        var area = $("#" + addressContainerId + " .areaFIASLabel").val();
        var city = $("#" + addressContainerId + " .cityFIASLabel").val();
        var locality = $("#" + addressContainerId + " .localityFIASLabel").val();
        var street = $("#" + addressContainerId + " .streetFIASLabel").val();
        var house = $("#" + addressContainerId + " .houseLabel").val();
        var housing = $("#" + addressContainerId + " .housingLabel").val();
        var building = $("#" + addressContainerId + " .buildingLabel").val();
        var appartament = $("#" + addressContainerId + " .appartmentLabel").val();

        var fullAddress = "";
        if (region) {
            fullAddress = fullAddress + region;
        }
        if (area) {
            fullAddress = fullAddress + ", " + area;
        }
        if (city) {
            fullAddress = fullAddress + ", " + city;
        }
        if (locality) {
            fullAddress = fullAddress + ", " + locality;
        }
        if (street) {
            fullAddress = fullAddress + ", " + street;
        }
        if (house) {
            fullAddress = fullAddress + ", д " + house;
        }
        if (housing) {
            fullAddress = fullAddress + ", к " + housing;
        }
        if (building) {
            fullAddress = fullAddress + ", стр " + building;
        }
        if (appartament) {
            fullAddress =  fullAddress + ", кв "  + appartament;
        }

        $("#" + addressContainerId + " .fullAddressLabel").val(fullAddress);
    }
}

AddressProcessor.bind = function (addressContainerId) {
    AddressProcessor.bindRegion(addressContainerId);
    AddressProcessor.bindArea(addressContainerId);
    AddressProcessor.bindCity(addressContainerId);
    AddressProcessor.bindLocality(addressContainerId);
    AddressProcessor.bindStreet(addressContainerId);

    $(document).on('input', "#" + addressContainerId + " .regionFIASLabel", function (e) {
        AddressProcessor.clearArea(addressContainerId);
        AddressProcessor.clearLocality(addressContainerId);
        AddressProcessor.clearStreet(addressContainerId);
        AddressProcessor.buildFullAddress(addressContainerId);
    })

    $(document).on('input', "#" + addressContainerId + " .areaFIASLabel", function (e) {
        AddressProcessor.clearLocality(addressContainerId);
        AddressProcessor.clearStreet(addressContainerId);
        AddressProcessor.buildFullAddress(addressContainerId);
    })

    $(document).on('input', "#" + addressContainerId + " .cityFIASLabel", function (e) {
        AddressProcessor.clearLocality(addressContainerId);
        AddressProcessor.clearStreet(addressContainerId);
        AddressProcessor.buildFullAddress(addressContainerId);
    })

    $(document).on('input', "#" + addressContainerId + " .localityFIASLabel", function (e) {
        AddressProcessor.clearStreet(addressContainerId);
        AddressProcessor.buildFullAddress(addressContainerId);
    })

    $(document).on('input', "#" + addressContainerId + " .streetFIASLabel", function (e) {
        AddressProcessor.buildFullAddress(addressContainerId);
    })

    $(document).on('input', "#" + addressContainerId + " .houseLabel", function (e) {
        AddressProcessor.buildFullAddress(addressContainerId);
    })

    $(document).on('input', "#" + addressContainerId + " .housingLabel", function (e) {
        AddressProcessor.buildFullAddress(addressContainerId);
    })

    $(document).on('input', "#" + addressContainerId + " .buildingLabel", function (e) {
        AddressProcessor.buildFullAddress(addressContainerId);
    })

    $(document).on('input', "#" + addressContainerId + " .appartmentLabel", function (e) {
        AddressProcessor.buildFullAddress(addressContainerId);
    })
}