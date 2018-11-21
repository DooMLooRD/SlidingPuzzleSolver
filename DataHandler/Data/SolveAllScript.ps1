# Run a program supposed to solve the 15-puzzle in a batch mode using various
# strategies for searching the state space and applying them to all the initial
# states of the puzzle stored in files in the current directory.
#
# The names of the files storing the initial states of the puzzle should obey
# the following format:
#  size_depth_id.txt
# for example:
#  4x4_01_00001.txt
#
# Optional arguments:
#  -strategy STRATEGY
#    use only strategy STRATEGY for searching the state space.
#  -param PARAM
#    use only the particular order specified as PARAM for blind strategies or
#    the particular heuristic specified as PARAM for informed strategies.
#
# TODO: Change variable $Progcmd to match the command needed to invoke the
# actual program, using the absolute (or relative) path, for example:
#$Progcmd = 'D:\Studia\Semestr 5\SISE\Data\SlidingPuzzleCommandLine.exe'
#  $Progcmd = 'java -jar C:\Users\User\15puzzle\bin\solver.jar' (executable JAR file)
#  $Progcmd = python C:\Users\User\15puzzle\bin\solver.py' (Python file)

param([string]$strategy, [string]$param)

$Progcmd = "&'D:\Studia\Semestr 5\SISE\SlidingPuzzleSolver\SlidingPuzzleCommandLine\bin\Release\SlidingPuzzleCommandLine.exe'"
$Orders = @('RDUL', 'RDLU', 'DRUL', 'DRLU', 'LUDR',  'LURD', 'ULDR', 'ULRD')
$Heuristics = @('hamm', 'manh')
$InitFilenameRegex = '^[a-zA-Z0-9]+_[0-9]+_[0-9]+.txt$'

function RunProg([string]$strategy, [string]$param) {
    Get-ChildItem -File | Where-Object { $_.Name -match $InitFilenameRegex } | ForEach-Object {
        $FilenameRoot = '{0}_{1}_{2}' -f $_.BaseName, $strategy, $param.ToLower()
        $SolFilename = '{0}_sol.txt' -f $FilenameRoot
        $StatsFilename = '{0}_stats.txt' -f $FilenameRoot
        Invoke-Expression $('{0} {1} {2} {3} {4} {5}' -f $Progcmd, $strategy, $param, $_.Name, $SolFilename,
                            $StatsFilename)
    }
}

function RunBfs([string]$order) {
    Write-Host '===> Strategy: bfs <==='
    if ($order) {
        Write-Host " -> Order: $order"
        RunProg 'bfs' $order
    } else {
        foreach($o in $Orders) {
            Write-Host " -> Order: $o"
            RunProg 'bfs' $o
        }
    }
}

function RunDfs([string]$order) {
    Write-Host '===> Strategy: dfs <==='
    if ($order) {
        Write-Host " -> Order: $order"
        RunProg 'dfs' $order
    } else {
        foreach($o in $Orders) {
            Write-Host " -> Order: $o"
            RunProg 'dfs' $o
        }
    }
}

function RunIDDfs([string]$order) {
    Write-Host '===> Strategy: iddfs <==='
    if ($order) {
        Write-Host " -> Order: $order"
        RunProg 'iddfs' $order
    } else {
        foreach($o in $Orders) {
            Write-Host " -> Order: $o"
            RunProg 'iddfs' $o
        }
    }
}

function RunAstr([string]$heuristic) {
    Write-Host '===> Strategy: astr <==='
    if ($heuristic) {
        Write-Host " -> Heuristic: $heuristic"
        RunProg 'astr' $heuristic
    }
    foreach($h in $Heuristics) {
        Write-Host " -> Heuristic: $h"
        RunProg 'astr' $h
    }
}

function RunAll() {	
    RunIDDfs
	RunBfs
    RunDfs
    RunAstr
}

# Parse arguments
if (!$PSBoundParameters.ContainsKey('strategy') -and $PSBoundParameters.ContainsKey('param')) {
    Write-Error $("Argument 'param' requires argument 'strategy'.")
    exit 2
}

# Run a program solving the 15-puzzle using appropriate strategy/strategies
if (!$PSBoundParameters.ContainsKey('strategy')) {
	RunAll
} else {
    switch ($strategy.ToLower()) {
        'bfs'  { RunBfs $param }
        'dfs'  { RunDfs $param }
        'iddfs'  { RunIDDfs $param }
        'astr' { RunAstr $param }
        default {
            Write-Error $("Unrecognized strategy: '{0}'." -f $strategy)
            exit 1
        }
    }
}

Write-Host -NoNewLine 'Press any key to continue...';
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');