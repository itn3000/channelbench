# 1 = method
# 28 = toolchain
# 40 = IsSingleReader
# 39 = AllowSync
# 41 = Mean
# 37 = TaskNum

$fpath = ".\BenchmarkDotNet.Artifacts\results\ChannelQueueBench-report.csv"

g:/bin/xsv search -s 40 "True" $fpath | g:/bin/xsv sort -N -s "37" | g:/bin/xsv sort -s "1,28" | g:/bin/xsv select "1,28,37,39,40,41" > allowsync-sorted.csv
g:/bin/xsv search -s 39 "True" $fpath | g:/bin/xsv sort -N -s "1,28,37" | g:/bin/xsv select "1,28,37,39,40,41" > issinglereader-sorted.csv
g:/bin/xsv search -s 40 "True" $fpath | g:/bin/xsv search -s 39 "False" | g:/bin/xsv sort -N -s "37,28" | g:/bin/xsv select "1,28,37,39,40,41" > method-sorted.csv