const path = require('path');
var version = location.search.split('version=')[1];
var namespace = 'QuickStart.' + version.charAt(0).toUpperCase() + version.substr(1);
if(version === 'core') version = 'coreapp';

const baseNetAppPath = path.join(__dirname, '/src/'+ namespace +'/bin/Debug/net5.0');


process.env.EDGE_USE_CORECLR = 1;
process.env.EDGE_DEBUG = 1;

console.log(JSON.stringify(process.versions, null, 2));

if(version !== 'standard')
    process.env.EDGE_APP_ROOT = baseNetAppPath;

var edge = require('electron-edge-js');

var baseDll = path.join(baseNetAppPath, namespace + '.dll');

var localTypeName = namespace + '.LocalMethods';
var externalTypeName = namespace + '.ExternalMethods';

var getAppDomainDirectory = edge.func({
    assemblyFile: baseDll,
    typeName: localTypeName,
    methodName: 'GetAppDomainDirectory'
});

var getCurrentTime = edge.func({
    assemblyFile: baseDll,
    typeName: localTypeName,
    methodName: 'GetCurrentTime'
});

var useDynamicInput = edge.func({
    assemblyFile: baseDll,
    typeName: localTypeName,
    methodName: 'UseDynamicInput'
});

var getPerson = edge.func({
    assemblyFile: baseDll,
    typeName: externalTypeName,
    methodName: 'GetPersonInfo'
});

var getDelegate = edge.func({
    assemblyFile: baseDll,
    typeName: localTypeName,
    methodName: 'GetDelegateHandle'
});

var getComplexObject = edge.func({
    assemblyFile: baseDll,
    typeName: localTypeName,
    methodName: 'GetComplexObject'
});

var getFromThread = edge.func({
    assemblyFile: baseDll,
    typeName: localTypeName,
    methodName: 'GetFromThread'
});

var registerEventCallback = edge.func({
    assemblyFile: baseDll,
    typeName: localTypeName,
    methodName: 'RegisterEventCallback'
});


window.onload = function() {

    document.getElementById("ProcessVersions").innerHTML = `<pre>${JSON.stringify(process.versions, null, 2)}</pre>`

    getAppDomainDirectory('', function(error, result) {
        if (error) throw error;
        document.getElementById("GetAppDomainDirectory").innerHTML = result;
    });

    getCurrentTime('', function(error, result) {
        if (error) throw error;
        document.getElementById("GetCurrentTime").innerHTML = result;
    });

    useDynamicInput('Node.Js', function(error, result) {
        if (error) throw error;
        document.getElementById("UseDynamicInput").innerHTML = result;
    });

    getPerson('', function(error, result) {
        //if (error) throw JSON.stringify(error);
        document.getElementById("GetPersonInfo").innerHTML = result;
    });

    getDelegate('', function(error, result){
        if(error) throw error;
        
        console.log(result);
        var getTimeFunc = result.GetCurrentTime;

        getTimeFunc('', function(error, result){
            if(error) throw error;
            console.log(result);
            document.getElementById("GetCurrentTimeFromDelegate").innerHTML = result;
        });
    })

    getComplexObject('', function(error, result){
        if(error) throw error;
        
        console.log(result);
        document.getElementById("GetDictionary").innerHTML = result['test'] + "\r\n" + result['now'] + "\r\n" + result['long'];
    })

    getFromThread('', function(error, result){
        if(error) throw error;
        
        console.log(result);
    })

    registerEventCallback({EventCallback: OnTimerElapsed}, function(error, result){
        if(error) throw error;
        console.log('Registering done')
    })
};

function OnTimerElapsed(signalTime, callback ){

    document.getElementById("OnTimerElapsed").innerHTML = signalTime
    console.log(signalTime)
    
    // result of method
    var result = 123;
    
    // "complete method" with (error, result) return values reported via the given callback
    callback(null, result);
    
    // error case
    // callback(new Error("Exception in OnTimeElapsed"), result);
}

