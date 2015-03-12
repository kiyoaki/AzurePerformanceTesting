# AzurePerformanceTesting

### Testing results

#### Azure Websites
```
Standard L size
```

#### S2(50DTU) EntityFramework
```
2015-03-11T11:04:24  PID[3920] Information 10000rows written in 34284ms
2015-03-11T11:04:34  PID[3920] Information 10000rows read in 10469ms
```

#### S2(50DTU) Dapper
```
2015-03-11T11:06:42  PID[3920] Information 10000rows written in 32970ms
2015-03-11T11:06:45  PID[3920] Information 10000rows read in 3422ms
```

#### P1(100DTU) EntityFramework
```
2015-03-11T11:18:45  PID[3920] Information 10000rows written in 26694ms
2015-03-11T11:18:51  PID[3920] Information 10000rows read in 5862ms
```

#### P1(100DTU) Dapper
```
2015-03-11T11:17:42  PID[3920] Information 10000rows written in 26100ms
2015-03-11T11:17:43  PID[3920] Information 10000rows read in 1725ms
```

#### P2(200DTU) EntityFramework
```
2015-03-11T11:37:28  PID[3920] Information 10000rows written in 13480ms
2015-03-11T11:37:33  PID[3920] Information 10000rows read in 5120ms
```

#### P2(200DTU) Dapper
```
2015-03-11T11:36:49  PID[3920] Information 10000rows written in 13192ms
2015-03-11T11:36:51  PID[3920] Information 10000rows read in 1740ms
```

#### P3(800DTU) EntityFramework
```
2015-03-11T12:21:09  PID[3920] Information 10000rows written in 11046ms
2015-03-11T12:21:15  PID[3920] Information 10000rows read in 5221ms
```

#### P3(800DTU) Dapper
```
2015-03-11T12:20:24  PID[3920] Information 10000rows written in 6216ms
2015-03-11T12:20:26  PID[3920] Information 10000rows read in 2112ms
```

#### P3(800DTU) Raw ADO.NET
```
2015-03-11T12:21:41  PID[3920] Information 10000rows written in 4968ms
2015-03-11T12:21:44  PID[3920] Information 10000rows read in 2137ms
```
