require 'bundler/setup'
require 'fuburake'

@solution = FubuRake::Solution.new do |sln|
  sln.compile = {
    :solutionfile => 'src/Sabatoast.Puller.sln'
  }

  sln.assembly_info = {
    :product_name => "Sabatoast.Puller",
    :copyright => 'Copyright 2013 Matthew L. Smith et al. All rights reserved.'
  }

  sln.ripple_enabled = true
  sln.fubudocs_enabled = false

  sln.integration_test = []
  sln.ci_steps = []

  sln.defaults = []
end
